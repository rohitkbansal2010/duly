// <copyright file="AuditException.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Runtime.Serialization;

namespace Duly.Clinic.Fhir.Adapter.Audit
{
    [Serializable]
    public class AuditException : Exception
    {
        public AuditException()
        {
        }

        public AuditException(string message)
            : base(message)
        {
        }

        public AuditException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AuditException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}