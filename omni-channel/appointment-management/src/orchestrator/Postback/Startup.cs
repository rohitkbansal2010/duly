// <copyright file="Startup.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Azure.Identity;
using Duly.Common.Infrastructure.Components;
using Duly.Common.Security.Interfaces;
using Duly.Common.Security.Services;
using Duly.OmniChannel.Orchestrator.Appointment.Common.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Duly.OmniChannel.Orchestrator.Appointment.Postback
{
    /// <summary>
    /// Represents the entry point class of the isolated host.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Startup
    {
        /// <summary>
        /// Represents the configuration key for Key Vault host URL.
        /// </summary>
        private const string VaultConfigurationKey = "KeyVaultHostUrl";

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
                .ConfigureAppConfiguration(
                    (context, config) =>
                    {
                        var builtConfig = config.Build();

                        string keyVaultHostUrl = builtConfig[VaultConfigurationKey];

                        if (!string.IsNullOrEmpty(keyVaultHostUrl))
                        {
                            config.AddAzureKeyVault(
                                new Uri(keyVaultHostUrl),
                                new DefaultAzureCredential());
                        }
                    })
                .ConfigureServices(
                    (context, services) =>
                    {
                        services
                            .AddSingleton<INumberObfuscator, NumberObfuscator>()
                            .AddSingleton<ITelemetryInitializer, TelemetryInitializer>()
                            .AddIngestionClient(context.Configuration);
                    })
                .Build();

            await host.RunAsync();
        }
    }
}