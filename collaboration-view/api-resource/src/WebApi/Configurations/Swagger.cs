// -----------------------------------------------------------------------
// <copyright file="Swagger.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.Resource.Api.Contracts;
using Duly.Common.Annotations.Configurations;
using Duly.Common.Infrastructure.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;

namespace Duly.CollaborationView.Resource.Api.Configurations
{
    public static class Swagger
    {
        private const string ApiVersion = "v1";
        private const string ApiTitle = "duly-resource-api";
        private const string ApiDescription = "Play a role of a data access layer for low-level communication with the Application Storage and other auxiliary data suppliers";
        private const string ApiName = "Resource API";

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerCommon(
                new SwaggerCommonParameters
                {
                    ApiVersion = ApiVersion,
                    ApiTitle = ApiTitle,
                    ApiDescription = ApiDescription,
                    FaultResponseType = typeof(FaultResponse)
                },
                options =>
                {
                    var initSwaggerSecurityParameters = SwaggerCommon.ReadSwaggerSecurityParametersFromConfiguration(configuration);
                    if (initSwaggerSecurityParameters.IsValid())
                    {
                        SwaggerCommon.AddOauth2SecurityDefinition(options, initSwaggerSecurityParameters);
                    }
                });

            return services.AddSwaggerExamplesFromAssemblyOf<UiConfiguration>();
        }

        public static IApplicationBuilder UseResourceSwagger(this IApplicationBuilder app)
        {
            return app.UseSwaggerCommon(ApiVersion, ApiName);
        }
    }
}
