// <copyright file="ResourceContextException.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Runtime.Serialization;

namespace Duly.CollaborationView.Resource.Api.Contexts
{
    [Serializable]
    public class ResourceContextException : Exception
    {
        public ResourceContextException()
        {
        }

        public ResourceContextException(string message)
            : base(message)
        {
        }

        public ResourceContextException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ResourceContextException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}