// <copyright file="ConceptNotMappedException.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    [Serializable]
    public class ConceptNotMappedException : Exception
    {
        public ConceptNotMappedException(string message)
            : base(message)
        {
        }

        protected ConceptNotMappedException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}