// <copyright file="Startup.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Infrastructure.Extensions;
using Duly.OmniChannel.Orchestrator.Appointment.Common.Extensions;
using Hangfire;
using Hangfire.Console;
using Hangfire.Console.Extensions;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Duly.OmniChannel.Orchestrator.Appointment.Workflow
{
    /// <summary>
    /// Represents the startup methods of the application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="configuration">An instance of the configuration object.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the instance of the configuration object.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures the required services for the application.
        /// </summary>
        /// <param name="services">An instance of the services collection.</param>
        /// <remarks>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </remarks>
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHangfire(
                    (serviceProvider, configuration) =>
                        configuration
                            .UseConsole()
                            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                            .UseSimpleAssemblyNameTypeSerializer()
                            .UseRecommendedSerializerSettings()
                            .UseMemoryStorage())
                .AddHangfireConsoleExtensions()
                .AddHangfireServer();

            services
                .UseApplicationInsightsTelemetryWorkerService(Configuration)
                .AddIngestionClient(Configuration);

            services
                .AddHealthChecks()
                .AddHangfire(
                    option =>
                    {
                        option.MaximumJobsFailed = null;
                        option.MinimumAvailableServers = 1;
                    });
        }

        /// <summary>
        /// Configures the HTTP request pipeline for the application.
        /// </summary>
        /// <param name="app">An instance of the application builder object.</param>
        /// <param name="env">An instance of the web host environment object.</param>
        /// <param name="jobManager">An instance of <see cref="IJobManager" /> object.</param>
        /// <param name="recurringJobManager">An instance of <see cref="IRecurringJobManager" /> object.</param>
        /// <remarks>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </remarks>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IJobManager jobManager, IRecurringJobManager recurringJobManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
            });

            recurringJobManager
                .AddOrUpdate<Invitation>(
                    "appointment-management-invitation-job",
                    job => job.RunAsync(),
                    Configuration["ApplicationSettings:ScheduleExpression"]);
        }
    }
}