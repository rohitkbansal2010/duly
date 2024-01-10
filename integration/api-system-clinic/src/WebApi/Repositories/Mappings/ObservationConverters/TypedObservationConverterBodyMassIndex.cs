// <copyright file="TypedObservationConverterBodyMassIndex.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Contracts;
using Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings.ObservationConverters
{
    internal class TypedObservationConverterBodyMassIndex : TypedObservationConverterBase
    {
        public override ObservationType ObservationType => ObservationType.BodyMassIndex;

        public override Coding[] SupportedCodings => new Coding[]
        {
            new(SystemLoinc, "39156-5")
        };
    }
}