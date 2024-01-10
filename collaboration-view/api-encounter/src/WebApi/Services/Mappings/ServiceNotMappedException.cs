// <copyright file="ServiceNotMappedException.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Runtime.Serialization;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    [Serializable]
    public class ServiceNotMappedException : Exception
    {
        public ServiceNotMappedException()
        {
        }

        public ServiceNotMappedException(string message)
            : base(message)
        {
        }

        public ServiceNotMappedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ServiceNotMappedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}