using Duly.Common.Infrastructure.Components;
using Duly.Common.Infrastructure.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlourishPlan_GeneratePDF
{
    public class Program
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

