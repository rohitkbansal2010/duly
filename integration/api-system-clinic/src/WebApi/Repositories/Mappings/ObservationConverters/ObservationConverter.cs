// <copyright file="ObservationConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using AutoMapper;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts;
using System.Collections.Generic;
using System.Linq;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings.ObservationConverters
{
    public class ObservationConverter :
        ITypeConverter<ObservationWithCompartments, Observation>,
        ITypeConverter<R4.Observation, Observation>,
        ITypeConverter<R4.Observation, ObservationLabResult>
    {
        private const string SystemLoinc = "http://loinc.org";

        public Observation Convert(ObservationWithCompartments source, Observation destination, ResolutionContext context)
        {
            return context.Mapper.Map<Observation>(source.Resource);
        }

        public Observation Convert(R4.Observation source, Observation destination, ResolutionContext context)
        {
            var codes = source.Code?.Coding
                .Where(x => x.System == SystemLoinc).Select(x => x.Code);

            var type = ConvertObservationType(codes);

            return type switch
            {
                ObservationType.BloodPressure => new TypedObservationConverterBloodPressure().Convert(source),
                ObservationType.BodyTemperature => new TypedObservationConverterBodyTemperature().Convert(source),
                ObservationType.OxygenSaturation => new TypedObservationConverterOxygenSaturation().Convert(source),
                ObservationType.HeartRate => new TypedObservationConverterHeartRate().Convert(source),
                ObservationType.RespiratoryRate => new TypedObservationConverterRespiratoryRate().Convert(source),
                ObservationType.PainLevel => new TypedObservationConverterPainLevel().Convert(source),
                ObservationType.BodyWeight => new TypedObservationConverterBodyWeight().Convert(source),
                ObservationType.BodyHeight => new TypedObservationConverterBodyHeight().Convert(source),
                ObservationType.BodyMassIndex => new TypedObservationConverterBodyMassIndex().Convert(source),
                _ => throw new ConceptNotMappedException($"Could not map {type} to a Observation Typed Converter")
            };
        }

        public ObservationLabResult Convert(R4.Observation source, ObservationLabResult destination, ResolutionContext context)
        {
           return TypedObservationConverterLabResult.ConvertToObservationLabResult(source);
        }

        private static ObservationType ConvertObservationType(IEnumerable<string> codes)
        {
            ObservationType? observationType = null;
            if (!codes.Any(code => code.ConvertFromLoincCodeToObservationType(out observationType)))
                throw new ConceptNotMappedException($"Could not map {nameof(codes)} to a ObservationType");

            return observationType.GetValueOrDefault();
        }
    }
}
