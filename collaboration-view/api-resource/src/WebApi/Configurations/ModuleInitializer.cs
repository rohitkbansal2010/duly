// <copyright file="ModuleInitializer.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Resource.Api.Repositories.Implementations;
using Duly.CollaborationView.Resource.Api.Repositories.Interfaces;
using Duly.CollaborationView.Resource.Api.Repositories.Mappings;
using Duly.UiConfig.Adapter.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Duly.CollaborationView.Resource.Api.Configurations
{
    public static class ModuleInitializer
    {
        public static IServiceCollection AddApiModules(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddUiConfigAdapter(configuration);

            services.AddTransient<IConfigurationRepository, ConfigurationRepository>();

            return services;
        }

        public static IServiceCollection AddApiMappings(this IServiceCollection services, IConfiguration configuration)
        {
            // Specifies additional configuration parameters and registries all the AutoMapper profiles.
            services.AddAutoMapper(
                cfg =>
                {
                    cfg.Advanced.AllowAdditiveTypeMapCreation = true;
                },
                typeof(AdapterContractsToProcessContractsProfile),
                typeof(ProcessContractsToAdapterContractsProfile));
            return services;
        }
    }
}
