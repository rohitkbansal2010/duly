// <copyright file="TypedObservationConverterBodyWeight.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings.ObservationConverters
{
    internal class TypedObservationConverterBodyWeight : TypedObservationConverterBase
    {
        public override ObservationType ObservationType => ObservationType.BodyWeight;

        public override Coding[] SupportedCodings => new Coding[]
        {
            new(SystemLoinc, "29463-7")
        };
    }
}