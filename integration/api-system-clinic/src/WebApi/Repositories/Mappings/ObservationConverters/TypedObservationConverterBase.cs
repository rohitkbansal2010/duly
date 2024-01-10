// <copyright file="TypedObservationConverterBase.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Extensions;
using Duly.Clinic.Fhir.Adapter.Extensions.Fhir;
using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings.ObservationConverters
{
    public abstract class TypedObservationConverterBase
    {
        protected const string SystemLoinc = "http://loinc.org";

        public abstract ObservationType ObservationType { get; }

        public abstract Coding[] SupportedCodings { get; }

        public virtual Observation Convert(R4.Observation source)
        {
            if ( source.Code == null || !AreSupportedCodingsValid(source.Code.Coding))
            {
                throw new ConceptNotMappedException("Expected coding not found");
            }

            return new()
            {
                Date = FindDate(source.Effective),
                Type = ObservationType,
                Components = BuildComponents(source)
            };
        }

        protected static ObservationMeasurement FindMeasurementFromQuantity(DataType value)
        {
            if (value is not Hl7.Fhir.Model.Quantity quantity)
                throw new ConceptNotMappedException("Unsupported type of DataType for ObservationMeasurement");

            return new ObservationMeasurement
            {
                Value = quantity.Value ?? default,
                Unit = quantity.Unit
            };
        }

        protected static DateTimeOffset FindDate(DataType statementEffective)
        {
            return statementEffective switch
            {
                FhirDateTime dateTime => dateTime.BuildDateTimeOffset(),
                _ => throw new ConceptNotMappedException("Could not map effective of observation")
            };
        }

        protected virtual bool AreSupportedCodingsValid(IEnumerable<Coding> codingsToValidate)
        {
            return SupportedCodings.Any(expectedCoding => codingsToValidate.Contains(expectedCoding, new CodingsComparer()));
        }

        protected virtual ObservationComponent[] BuildComponents(R4.Observation source)
        {
            return new ObservationComponent[]
            {
                new()
                {
                    Measurements = new[]
                    {
                        FindMeasurementFromQuantity(source.Value)
                    }
                }
            };
        }
    }
}
