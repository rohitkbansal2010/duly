// <copyright file="ApiException.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.CollaborationView.CdcClient.Implementations
{
    public class ApiException : Exception
    {
        public ApiException(string message, int statusCode, Exception innerException)
            : base($"{message}\n\nStatus: {statusCode}\n", innerException)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; private set; }
    }
}