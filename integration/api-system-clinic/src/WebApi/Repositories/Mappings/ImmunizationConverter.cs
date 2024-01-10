// <copyright file="ImmunizationConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using AutoMapper;
using Duly.Clinic.Contracts;
using Hl7.Fhir.Model;
using System;
using System.Linq;
using Quantity = Duly.Clinic.Contracts.Quantity;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps https://www.hl7.org/fhir/immunization.html into <see cref="Immunization"/>.
    /// </summary>
    public class ImmunizationConverter : ITypeConverter<R4.Immunization, Immunization>
    {
        public Immunization Convert(R4.Immunization source, Immunization destination, ResolutionContext context)
        {
            return new Immunization
            {
                Id = source.Id,
                Status = context.Mapper.Map<ImmunizationStatus>(source.Status),
                Dose = context.Mapper.Map<Quantity>(source.DoseQuantity),
                OccurrenceDateTime = context.Mapper.Map<DateTimeOffset>(source.Occurrence),
                Notes = source.Note?.Any() ?? false ? source.Note.Select(n => n.Text.Value).ToArray() : null,
                Vaccine = context.Mapper.Map<Vaccine>(source.VaccineCode),
                StatusReason = GetStatusReason(source.StatusReason)
            };
        }

        private static ImmunizationStatusReason GetStatusReason(CodeableConcept source)
        {
            return string.IsNullOrWhiteSpace(source?.Text) ? null : new ImmunizationStatusReason { Reason = source.Text };
        }
    }
}