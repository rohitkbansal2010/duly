// <copyright file="ModuleInitializer.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Configuration;
using Duly.Ngdp.Api.Repositories.Implementations;
using Duly.Ngdp.Api.Repositories.Implementations.Dashboard;
using Duly.Ngdp.Api.Repositories.Interfaces;
using Duly.Ngdp.Api.Repositories.Interfaces.Dashboard;
using Duly.Ngdp.Api.Repositories.Mappings.Converters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Duly.Ngdp.Api.Configurations
{
    public static class ModuleInitializer
    {
        public static IServiceCollection AddApiMappings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<NgdpTimeZone>(configuration.GetSection(nameof(NgdpTimeZone)));
            services.AddSingleton<ITimeZoneConverter, TimeZoneConverter>();

            services.AddNgdpAdapter(configuration);

            services.AddTransient<IAppointmentRepository, NgdpAppointmentRepository>();
            services.AddTransient<IImmunizationRepository, NgdpImmunizationRepository>();
            services.AddTransient<IRecommendedProviderRepository, NgdpRecommendedProviderRepository>();
            services.AddTransient<IPatientRepository, NgdpPatientRepository>();
            services.AddTransient<ILabDetailsRepository, NgdpLabDetailsRepository>();
            services.AddTransient<IProviderRepository, NgdpProviderRepository>();
            services.AddTransient<IPharmacyRepository, NgdpPharmacyRepository>();
            services.AddTransient<ISlotDataRepository, NgdpSlotDataRepository>();
            services.AddTransient<IDashboardRepository, NgdpDashboardRepository>();
            services.AddAutoMapper(typeof(NgdpToSystemApiContractsProfile));

            return services;
        }
    }
}