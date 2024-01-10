// <copyright file="RepositoryNotMappedException.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Runtime.Serialization;

namespace Duly.CollaborationView.Resource.Api.Repositories.Mappings
{
    [Serializable]
    public class RepositoryNotMappedException : Exception
    {
        public RepositoryNotMappedException()
        {
        }

        public RepositoryNotMappedException(string message)
            : base(message)
        {
        }

        public RepositoryNotMappedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected RepositoryNotMappedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}