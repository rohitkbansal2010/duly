// <copyright file="LogAnalyticsExtensions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Audit.Ingestion.Configuration;
using Duly.Clinic.Audit.Ingestion.Interfaces;
using Duly.Clinic.Audit.Ingestion.Services;
using LogAnalytics.Client;
using LogAnalytics.Client.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Duly.Clinic.Audit.Ingestion.Extensions
{
    /// <summary>
    /// Adds LogAnalytics services.
    /// </summary>
    public static class LogAnalyticsExtensions
    {
        /// <summary>
        /// Adds CloudEventLogAdapter and all related services.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <param name="configuration">Configuration.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddLogAnalytics(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .Configure<LogAnalyticsClientOptions>(configuration.GetSection(nameof(LogAnalyticsClientOptions)))
                .Configure<LogAnalyticsOptions>(configuration.GetSection(nameof(LogAnalyticsOptions)));

            services
                .AddTransient<ILogAnalyticsEntryBuilder, LogAnalyticsEntryBuilder>()
                .AddTransient<ICloudEventLogAdapter, CloudEventLogAdapter>()
                .AddHttpClient<ILogAnalyticsClient, LogAnalyticsClient>();

            return services;
        }
    }
}
