using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Rawdata.Data;
using Rawdata.Data.Models;
using Rawdata.Data.Services;
using Rawdata.Data.Services.Interfaces;
using Rawdata.Service.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace Rawdata.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMapper>(_ => CreateMapper());

            services
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Formatting = Formatting.Indented;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(
                options => options.SwaggerDoc("api", new Info { Title = "Stackoverflow API" })
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/api/swagger.json", "Stackoverflow API");
                options.RoutePrefix = "api";
            });

            // app.UseHttpsRedirection();
            app.UseMvc();
        }

        private IMapper CreateMapper() =>
            new MapperConfiguration(cfg => { cfg.CreateMap<User, UserDto>(); }).CreateMapper();
    }
}
