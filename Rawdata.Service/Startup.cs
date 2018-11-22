using System;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Rawdata.Data;
using Rawdata.Data.Services;
using Rawdata.Data.Services.Interfaces;
using Rawdata.Service.Profiles;
using Swashbuckle.AspNetCore.Swagger;

namespace Rawdata.Service
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddJsonOptions(options => {
                    // Avoid infinite loop caused by circular referencing of relationships with EF
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<DataContext>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("L0onhppuCM1lMTwiEYe8667BZ-Bd8C22ETjdsdRm5NU")
                        ),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Inject ActionContextAccessor to retrieve UrlHelper
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // Inject UrlHelper so it can used in out of controller context
            services.AddScoped<IUrlHelper>(p => new UrlHelper(p.GetService<IActionContextAccessor>().ActionContext));

            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<ISearchResultService, SearchResultService>();

            // Create and inject DtoMapper
            services.AddScoped<IMapper>(p => CreateMapper(p));


            services.AddSwaggerGen(
                options => options.SwaggerDoc("api", new Info { Title = "Stackoverflow API" })
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/api/swagger.json", "Stackoverflow API");
                options.RoutePrefix = "api";
            });

            app.UseAuthentication();
            app.UseMvc();
        }

        private IMapper CreateMapper(IServiceProvider provider)
        {
            // Create MapperConfiguration and add profiles for entities
            var mapperCfg = new MapperConfiguration(cfg => {
                // Retrieve UrlHelper from the container
                var url = provider.GetService<IUrlHelper>();

                // Add profiles and supply it to the profile contructors
                cfg.AddProfile(new AnswerProfile(url));
                cfg.AddProfile(new AuthorProfile(url));
                cfg.AddProfile(new CommentProfile(url));
                cfg.AddProfile(new QuestionProfile(url));
                cfg.AddProfile(new UserProfile(url));
                cfg.AddProfile(new MarkedCommentProfile(url));
                cfg.AddProfile(new MarkedQuestionProfile(url));
                cfg.AddProfile(new MarkedAnswerProfile(url));
            });

            return mapperCfg.CreateMapper();
        }
    }
}
