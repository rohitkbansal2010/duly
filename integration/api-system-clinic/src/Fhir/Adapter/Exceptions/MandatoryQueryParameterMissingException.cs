// <copyright file="MandatoryQueryParameterMissingException.cs" company="Duly Health and Care">
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
    public class MandatoryQueryParameterMissingException : Exception
    {
        public MandatoryQueryParameterMissingException()
        {
        }

        public MandatoryQueryParameterMissingException(string message)
            : base(message)
        {
        }

        public MandatoryQueryParameterMissingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MandatoryQueryParameterMissingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
