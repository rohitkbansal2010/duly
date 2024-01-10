// <copyright file="EncounterContextException.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Runtime.Serialization;

namespace Duly.CollaborationView.Encounter.Api.Contexts
{
    [Serializable]
    public class EncounterContextException : Exception
    {
        public EncounterContextException()
        {
        }

        public EncounterContextException(string message)
            : base(message)
        {
        }

        public EncounterContextException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected EncounterContextException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}