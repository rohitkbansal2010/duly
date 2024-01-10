// <copyright file="TypedObservationConverterBloodPressure.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Contracts;
using Hl7.Fhir.Model;
using System.Collections.Generic;
using System.Linq;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings.ObservationConverters
{
    public class TypedObservationConverterBloodPressure : TypedObservationConverterBase
    {
        public static string[] SupportedComponentCodes => new[] { "8480-6", "8462-4" };

        public override ObservationType ObservationType => ObservationType.BloodPressure;

        public override Coding[] SupportedCodings => new[]
        {
            new Coding(SystemLoinc, "85354-9")
        };

        protected override ObservationComponent[] BuildComponents(R4.Observation source)
        {
            if (source.Component.Count < 2)
            {
                throw new ConceptNotMappedException("Component Count is too low");
            }

            return ConvertComponents(source.Component);
        }

        private static ObservationComponent[] ConvertComponents(IEnumerable<R4.Observation.ComponentComponent> components)
        {
            return components.Select(x => new ObservationComponent
            {
                Type = ConvertComponentType(x.Code),
                Measurements = new[]
                {
                    FindMeasurementFromQuantity(x.Value)
                }
            }).ToArray();
        }

        private static ObservationComponentType ConvertComponentType(CodeableConcept codeableConcept)
        {
            if (codeableConcept == null)
            {
                throw new ConceptNotMappedException("Could not map type of observation component");
            }

            var code = codeableConcept.Coding.FirstOrDefault(x => SupportedComponentCodes.Contains(x.Code) && x.System == SystemLoinc);

            return code?.Code switch
            {
                "8480-6" => ObservationComponentType.Systolic,
                "8462-4" => ObservationComponentType.Diastolic,
                _ => throw new ConceptNotMappedException("Could not map type of observation component"),
            };
        }
    }
}
