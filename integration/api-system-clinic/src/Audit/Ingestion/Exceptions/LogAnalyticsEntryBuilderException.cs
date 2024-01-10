// <copyright file="LogAnalyticsEntryBuilderException.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Runtime.Serialization;

namespace Duly.Clinic.Audit.Ingestion.Exceptions
{
    /// <summary>
    /// This exception is thrown when something went wrong during the building of the <see cref="LogAnalyticsEntry" />.
    /// </summary>
    [Serializable]
    public class LogAnalyticsEntryBuilderException : Exception
    {
        public LogAnalyticsEntryBuilderException()
            : base()
        {
        }

        public LogAnalyticsEntryBuilderException(string message)
            : base(message)
        {
        }

        public LogAnalyticsEntryBuilderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected LogAnalyticsEntryBuilderException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
