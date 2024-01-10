// <copyright file="TypedObservationConverterBodyTemperature.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Contracts;
using Hl7.Fhir.Model;
using System.Linq;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings.ObservationConverters
{
    public class TypedObservationConverterBodyTemperature : TypedObservationConverterBase
    {
        private const string ExtensionSystem = "http://unitsofmeasure.org";
        private const string ExtensionUrl = "http://open.epic.com/FHIR/STU3/StructureDefinition/temperature-in-fahrenheit";
        private const string ExtensionCode = "[degF]";

        public override ObservationType ObservationType => ObservationType.BodyTemperature;

        public override Coding[] SupportedCodings => new Coding[] { new(SystemLoinc, "8310-5") };

        protected override ObservationComponent[] BuildComponents(R4.Observation source)
        {
            var extensionValue = FindMeasurementFromExtension(source);
            var originalValue = FindMeasurementFromQuantity(source.Value);

            return new ObservationComponent[]
            {
                new()
                {
                    Measurements = extensionValue == null
                        ? new[] { originalValue }
                        : new[] { originalValue, extensionValue }
                }
            };
        }

        private static ObservationMeasurement FindMeasurementFromExtension(R4.Observation observation)
        {
            var extensionBodyTemperature = observation.Extension
                .Where(extension => extension.Url == ExtensionUrl)
                .Select(extension => extension.Value)
                .OfType<Hl7.Fhir.Model.Quantity>()
                .FirstOrDefault(x => x.System == ExtensionSystem && x.Code == ExtensionCode);

            return extensionBodyTemperature == null ? null : FindMeasurementFromQuantity(extensionBodyTemperature);
        }
    }
}
