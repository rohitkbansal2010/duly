// <copyright file="ExceptionHandler.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using Wipfli.Adapter.Client;

namespace Wipfli.Adapter.Exceptions
{
    public static class ExceptionHandler
    {
        public static int? ExtendHandling(Exception exception)
        {
            return exception switch
            {
                ApiException apiException => apiException.StatusCode,
                _ => null
            };
        }
    }
}
