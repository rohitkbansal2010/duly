// <copyright file="Startup.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Audit.Ingestion.Extensions;
using Duly.Common.Infrastructure.Components;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Duly.Clinic.Audit.Ingestion
{
    /// <summary>
    /// Represents the entry point class of the isolated host.
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// The main entry point for the the isolated host.
        /// </summary>
        /// <returns>
        /// Void result when the isolated host created.
        /// </returns>
        public static async Task Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureAppConfiguration((hostContext, builder) =>
                {
                    if (hostContext.HostingEnvironment.IsDevelopment())
                    {
                        builder.AddJsonFile("local.settings.json");
                    }
                })
                .ConfigureServices(
                    (context, services) =>
                    {
                        services
                            .AddSingleton<ITelemetryInitializer, TelemetryInitializer>()
                            .AddLogAnalytics(context.Configuration);
                    })
                .Build();

            await host.RunAsync();
        }
    }
}