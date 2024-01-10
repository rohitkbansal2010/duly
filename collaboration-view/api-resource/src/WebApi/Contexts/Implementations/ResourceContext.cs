// <copyright file="ResourceContext.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Resource.Api.Contexts.Interfaces;
using Duly.Common.Infrastructure.Constants;
using Microsoft.AspNetCore.Http;
using System;

namespace Duly.CollaborationView.Resource.Api.Contexts.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IResourceContext"/>
    /// </summary>
    public class ResourceContext : IResourceContext
    {
        private const string XCorrelationIdIsNotSpecifiedError = "The request does not contain 'x-correlation-id'.";

        private readonly IHttpContextAccessor _accessor;

        public ResourceContext(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Guid GetXCorrelationId()
        {
            if (_accessor.HttpContext != null
                && _accessor.HttpContext.Response.Headers.TryGetValue(
                    ParameterNames.XCorrelationIdHeader,
                    out var headerValue)
                && Guid.TryParse(headerValue.ToString(), out var correlationId))
            {
                return correlationId;
            }

            throw new ResourceContextException(XCorrelationIdIsNotSpecifiedError);
        }
    }
}
