// <copyright file="ContradictoryQueryException.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Runtime.Serialization;

namespace Duly.Clinic.Fhir.Adapter.Exceptions
{
    /// <summary>
    /// This is exception is used when known mandatory parameter for Epic query is missing.
    /// </summary>
    [Serializable]
    public class ContradictoryQueryException : Exception
    {
        public ContradictoryQueryException()
        {
        }

        public ContradictoryQueryException(string message)
            : base(message)
        {
        }

        public ContradictoryQueryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ContradictoryQueryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
