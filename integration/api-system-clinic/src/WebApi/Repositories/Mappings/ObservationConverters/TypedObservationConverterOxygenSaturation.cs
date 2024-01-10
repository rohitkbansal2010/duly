// <copyright file="TypedObservationConverterOxygenSaturation.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings.ObservationConverters
{
    internal class TypedObservationConverterOxygenSaturation : TypedObservationConverterBase
    {
        public override ObservationType ObservationType => ObservationType.OxygenSaturation;

        public override Coding[] SupportedCodings => new[]
        {
            new Coding(SystemLoinc, "2708-6"),
            new Coding(SystemLoinc, "59408-5")
        };
    }
}