// <copyright file="ModuleInitializer.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.DataAccess.Contexts.Implementations;
using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Implementations;
using Duly.Ngdp.Adapter.Adapters.Implementations.Dashboard;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Interfaces.Dashboard;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Duly.Ngdp.Adapter.Configuration
{
    public static class ModuleInitializer
    {
        private const string NgdpConnectionConnectionStringKey = "NgdpConnection";
        private const string DigitalDBConnectionConnectionStringKey = "DigitalDBConnection";

        /// <summary>
        /// Adds Configuration Adapter and all related services.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <param name="config">Configuration.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddNgdpAdapter(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IDapperContext, DapperContext>(_ => new DapperContext(config.GetConnectionString(NgdpConnectionConnectionStringKey)));
            services.AddTransient<ICVDapperContext, CVDapperContext>(_ => new CVDapperContext(config.GetConnectionString(DigitalDBConnectionConnectionStringKey)));
            services.AddTransient<IAppointmentAdapter, AppointmentAdapter>();
            services.AddTransient<IImmunizationAdapter, ImmunizationAdapter>();
            services.AddTransient<IRecommendedProviderAdapter, RecommendedProviderAdapter>();
            services.AddTransient<ILabDetailsAdapter, LabDetailsAdapter>();
            services.AddTransient<IProviderAdapter, ProviderAdapter>();
            services.AddTransient<IPharmacyAdapter, PharmacyAdapter>();
            services.AddTransient<ISlotDataAdapter, SlotDataAdapter>();
            services.AddTransient<IDashboardAdapter, DashboardAdapter>();
            return services;
        }
    }
}