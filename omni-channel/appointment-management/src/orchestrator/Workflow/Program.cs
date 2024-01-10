// <copyright file="Program.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Infrastructure.Components;
using Duly.Common.Infrastructure.Configurations;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace Duly.OmniChannel.Orchestrator.Appointment.Workflow
{
    /// <summary>
    /// Represents the entry point class of the application.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        private static IHost _host;

        /// <summary>
        /// Starts the application.
        /// </summary>
        /// <param name="args">The application arguments.</param>
        public static void Start(string[] args)
        {
            _host =
                DefaultWebHostBuilder
                    .Create<Startup>(args, vaultConfigurationKey: ConfigurationBuilderExtensions.GetValueOfVaultConfigurationKeyFromCommandLineArgs(args))
                    .Build();

            _host.Run();
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            _host.StopAsync().Wait();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">The application arguments.</param>
        private static void Main(string[] args) => Start(args);
    }
}