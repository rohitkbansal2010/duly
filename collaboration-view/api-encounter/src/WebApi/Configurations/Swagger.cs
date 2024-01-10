// -----------------------------------------------------------------------
// <copyright file="Swagger.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.Common.Annotations.Configurations;
using Duly.Common.Infrastructure.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;

namespace Duly.CollaborationView.Encounter.Api.Configurations
{
    public static class Swagger
    {
        private const string ApiVersion = "v1";
        private const string ApiTitle = "duly-encounter-api";
        private const string ApiDescription = "Perform set of encounter related operations requested from front-end view " +
                                              "and plays as conductor " +
                                              "between application " +
                                              "and operational storage " +
                                              "and front-end web components.";
        private const string ApiName = "Encounter API";

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerCommon(
                new SwaggerCommonParameters
            {
                ApiVersion = ApiVersion,
                ApiTitle = ApiTitle,
                ApiDescription = ApiDescription,
                FaultResponseType = typeof(FaultResponse)
            }, options =>
                {
                    var initSwaggerSecurityParameters = SwaggerCommon.ReadSwaggerSecurityParametersFromConfiguration(configuration);
                    if (initSwaggerSecurityParameters.IsValid())
                    {
                        SwaggerCommon.AddOauth2SecurityDefinition(options, initSwaggerSecurityParameters);
                    }
                });

            return services.AddSwaggerExamplesFromAssemblyOf<Allergy>();
        }

        public static IApplicationBuilder UseEncounterSwagger(this IApplicationBuilder app)
        {
            return app.UseSwaggerCommon(ApiVersion, ApiName);
        }
    }
}
