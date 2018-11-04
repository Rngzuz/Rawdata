using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Rawdata.Data;
using Rawdata.Data.Services;
using Rawdata.Data.Services.Interfaces;
using Rawdata.Service.Profiles;

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
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<DataContext>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(p => new UrlHelper(p.GetService<IActionContextAccessor>().ActionContext));

            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMapper>(p => CreateMapper(p));
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

            app.UseMvc();
        }

        private IMapper CreateMapper(IServiceProvider provider)
        {
            var mapperCfg = new MapperConfiguration(cfg => {
                var url = provider.GetService<IUrlHelper>();

                cfg.AddProfile(
                    new CommentProfile(url)
                );

                cfg.AddProfile(
                    new UserProfile(url)
                );
            });

            return mapperCfg.CreateMapper();
        }
    }
}
