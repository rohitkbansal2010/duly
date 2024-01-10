// <copyright file="EncounterContext.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.Common.Infrastructure.Constants;
using Microsoft.AspNetCore.Http;
using System;

namespace Duly.CollaborationView.Encounter.Api.Contexts.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IEncounterContext"/>
    /// </summary>
    public class EncounterContext : IEncounterContext
    {
        private const string XCorrelationIdIsNotSpecifiedError = "The request does not contain 'x-correlation-id'.";

        private readonly IHttpContextAccessor _accessor;

        public EncounterContext(IHttpContextAccessor accessor)
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

            throw new EncounterContextException(XCorrelationIdIsNotSpecifiedError);
        }
    }
}
