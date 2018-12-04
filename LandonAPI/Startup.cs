﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandonAPI.Filters;
using LandonAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSwag.AspNetCore;

namespace LandonAPI
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
            //Pull the propery from info section in appsetting.json file and create new instance of hoteinfo.
            //wraps the instance in to Ioption interface and puts Ioption in to service container and that can be injected to controller
            services.Configure<HotelInfo>(
                Configuration.GetSection("Info"));

            // Use In-memory database for quick developement and testing
            //TODO: Swap real database in Production
            services.AddDbContext<HotelApiDbContext>(options => options.UseInMemoryDatabase("HolidayInn"));
           


            services
                .AddMvc(options =>
                {
                    options.Filters.Add<JsonExceptionFilter>();
                    options.Filters.Add<RequiredHttpsOrCloseAttribe>();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new MediaTypeApiVersionReader();
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
            });

            services.AddCors(options =>
            {
                options.AddPolicy("Cors",
                    policy => policy.AllowAnyOrigin());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUiWithApiExplorer(options =>
                {
                    options.GeneratorSettings
                        .DefaultPropertyNameHandling
                    = NJsonSchema.PropertyNameHandling.CamelCase;
                });
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors("Cors");
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
