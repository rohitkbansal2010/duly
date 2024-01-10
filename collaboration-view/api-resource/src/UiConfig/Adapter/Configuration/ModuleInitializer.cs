// -----------------------------------------------------------------------
// <copyright file="ModuleInitializer.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.Common.DataAccess.Contexts.Implementations;
using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.UiConfig.Adapter.Implementations;
using Duly.UiConfig.Adapter.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Duly.UiConfig.Adapter.Configuration
{
    public static class ModuleInitializer
    {
        private const string UiConfigConnectionStringKey = "CollaborationViewMetadataConnection";

        /// <summary>
        /// Adds Configuration Adapter and all related services.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <param name="config">Configuration.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddUiConfigAdapter(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IDapperContext, DapperContext>(_ => new DapperContext(config.GetConnectionString(UiConfigConnectionStringKey)));
            services.AddTransient<IUiConfigAdapter, UiConfigAdapter>();

            return services;
        }
    }
}