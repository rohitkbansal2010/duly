// <copyright file="TypedObservationConverterLabResult.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Contracts;
using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings.ObservationConverters
{
    internal class TypedObservationConverterLabResult : TypedObservationConverterBase
    {
        private const string SystemObservationInterpretation = "http://hl7.org/fhir/ValueSet/observation-interpretation";

        public override ObservationType ObservationType => throw new NotSupportedException();

        public override Coding[] SupportedCodings => throw new NotSupportedException();

        public static ObservationLabResult ConvertToObservationLabResult(R4.Observation source)
        {
            return new ObservationLabResult
            {
                Id = source.Id,
                ComponentName = source.Code.Text,
                Components = new ObservationLabResultComponent[]
                {
                    new()
                    {
                        Measurements = ConvertValueQuantity(source.Value),
                        Interpretations = ConvertInterpretations(source.Interpretation),
                        ReferenceRange = ConvertReferenceRange(source.ReferenceRange),
                        ValueText = ConvertFhirString(source.Value),
                        Notes = source.Note.Select(x => x.Text.Value).ToArray()
                    }
                },
                Date = FindDate(source.Effective)
            };
        }

        private static ObservationLabResultInterpretation[] ConvertInterpretations(IEnumerable<CodeableConcept> fhirInterpretations)
        {
            var interpretations = fhirInterpretations
                .Where(x => x.Coding.Any(c => c.System.Equals(SystemObservationInterpretation, StringComparison.OrdinalIgnoreCase)))
                .Select(x => new ObservationLabResultInterpretation
                {
                    Code = x.Coding.First(c => c.System == SystemObservationInterpretation).Code,
                    Text = x.Text
                });

            return interpretations.ToArray();
        }

        private static ObservationLabResultReferenceRange[] ConvertReferenceRange(IEnumerable<R4.Observation.ReferenceRangeComponent> referenceRangeComponents)
        {
            var ranges = referenceRangeComponents
                .Where(component =>
                    component.Low != null &&
                    component.High != null)
                .Select(component => new ObservationLabResultReferenceRange
                {
                    Low = ConvertQuantity(component.Low),
                    High = ConvertQuantity(component.High),
                    Text = component.Text
                });

            return ranges.ToArray();
        }

        private static ObservationLabResultMeasurement ConvertQuantity(Hl7.Fhir.Model.Quantity quantity)
        {
            return new ObservationLabResultMeasurement
            {
                Value = quantity.Value,
                Unit = quantity.Unit
            };
        }

        private static string ConvertFhirString(DataType value)
        {
            return value switch
            {
                CodeableConcept concept => concept.Text,
                FhirString fhirString => fhirString.Value,
                _ => null
            };
        }

        private static ObservationLabResultMeasurement[] ConvertValueQuantity(DataType value)
        {
            return value switch
            {
                Hl7.Fhir.Model.Quantity quantity => new[] { ConvertQuantity(quantity) },
                _ => null
            };
        }
    }
}