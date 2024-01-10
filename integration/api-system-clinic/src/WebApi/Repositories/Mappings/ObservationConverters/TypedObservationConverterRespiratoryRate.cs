// <copyright file="TypedObservationConverterRespiratoryRate.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings.ObservationConverters
{
    internal class TypedObservationConverterRespiratoryRate : TypedObservationConverterBase
    {
        public override ObservationType ObservationType => ObservationType.RespiratoryRate;

        public override Coding[] SupportedCodings => new Coding[]
        {
            new(SystemLoinc, "9279-1")
        };
    }
}