// <copyright file="DosageConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias stu3;

using AutoMapper;
using Duly.Clinic.Contracts;
using Hl7.Fhir.Model;

using STU3 = stu3::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps
    ///     https://www.hl7.org/fhir/dosage.html
    /// into <see cref="Dosage"/>.
    /// </summary>
    public class DosageConverter : ITypeConverter<STU3.Dosage, Dosage>
    {
        public Dosage Convert(STU3.Dosage source, Dosage destination, ResolutionContext context)
        {
            return new Dosage
            {
                AsNeeded = FindAsNeeded(source.AsNeeded),
                Timing = context.Mapper.Map<Timing>(source.Timing),
                DoseQuantity = FindQuantity(source.Dose, context.Mapper),
                PatientInstruction = source.PatientInstruction,
                Text = source.Text
            };
        }

        private static bool FindAsNeeded(DataType asNeeded)
        {
            return asNeeded switch
            {
                null => false,
                FhirBoolean asNeededBoolean => asNeededBoolean.Value.GetValueOrDefault(),
                _ => throw new ConceptNotMappedException("Could not map AsNeeded")
            };
        }

        private static Contracts.Quantity FindQuantity(DataType dose, IRuntimeMapper contextMapper)
        {
            return dose switch
            {
                null => default,
                Hl7.Fhir.Model.Quantity quantity => contextMapper.Map<Contracts.Quantity>(quantity),
                _ => default //TODO refactor to handle Hl7.Fhir.Model.Range
            };
        }
    }
}