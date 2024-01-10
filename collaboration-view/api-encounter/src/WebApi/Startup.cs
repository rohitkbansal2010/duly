// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.CdcClient.Configurations;
using Duly.CollaborationView.Encounter.Api.Configurations;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Contracts.ConfigurationSettings;
using Duly.Common.Cache.Extensions;
using Duly.Common.Infrastructure.Configurations;
using Duly.Common.Infrastructure.Constants;
using Duly.Common.Infrastructure.Extensions;
using Duly.Common.Security.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Duly.CollaborationView.Encounter.Api
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
                .AddWebApiProtection(Configuration, addProtectedWebApiCallsProtectedWebApiScenario: true);

            //Enables solution wide enum to string conversion
            services
                .AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.ReplaceDefaultInvalidModelStateResponseFactory();
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            services.AddSwagger(Configuration);
            services
                .AddCompression()
                .AddHttpContextAccessor();

            services.AddAccountIdentityService();

            services.AddRedisCacheClient(Configuration);
            services.AddCdcApiClient(Configuration);

            services.AddClinicApiClients(Configuration);
            services.AddNgdpApiClients(Configuration);

            services.AddApiModules(Configuration);
            services.AddApiMappings(Configuration);

            services.AddApiConfiguration(Configuration);

            services.Configure<IngestionSettings>(Configuration.GetSection(nameof(IngestionSettings)));
            services.Configure<Authentication>(Configuration.GetSection(nameof(Authentication)));

            services.AddControllers().AddControllersAsServices();
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

            app.UseEncounterSwagger();

            app.UseJsonExceptionHandling(ModuleInitializer.SetupErrorHandlingAction);
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
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
