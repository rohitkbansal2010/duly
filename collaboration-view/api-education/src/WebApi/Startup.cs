// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.Common.Infrastructure.Configurations;
using Duly.Common.Infrastructure.Constants;
using Duly.Common.Infrastructure.Extensions;
using Duly.Common.Security.Extensions;
using Duly.Education.Api.Configurations;
using Duly.Education.Api.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace Duly.Education.Api
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
            services
                .AddCors()
                .AddMemoryCache()
                .AddHealthChecks();

            services
                .UseApplicationInsightsTelemetry(Configuration)
                .AddWebApiProtection(Configuration);

            //Enables solution wide enum to string conversion
            services
                .AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));

            services.AddSwagger();

            services
                .AddCompression()
                .AddHttpContextAccessor();

            services.AddAccountIdentityService();

            services.AddTransient<IExamplesProvider<IEnumerable<EducationalMaterial>>, EducationalMaterialExampleProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsEnvironment(EnvironmentNames.NonProduction))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseEducationSwagger();

            app.UseJsonExceptionHandling();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseResponseCompression();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
