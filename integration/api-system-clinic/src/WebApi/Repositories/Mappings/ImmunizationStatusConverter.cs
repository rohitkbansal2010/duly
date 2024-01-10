// <copyright file="ImmunizationStatusConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using AutoMapper;
using Duly.Clinic.Contracts;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps https://www.hl7.org/fhir/valueset-immunization-status.html into <see cref="ImmunizationStatus"/>.
    /// </summary>
    public class ImmunizationStatusConverter : ITypeConverter<R4.Immunization.ImmunizationStatusCodes, ImmunizationStatus>
    {
        public ImmunizationStatus Convert(R4.Immunization.ImmunizationStatusCodes source, ImmunizationStatus destination, ResolutionContext context)
        {
            return source switch
            {
                R4.Immunization.ImmunizationStatusCodes.NotDone => ImmunizationStatus.NotDone,
                R4.Immunization.ImmunizationStatusCodes.Completed => ImmunizationStatus.Completed,
                R4.Immunization.ImmunizationStatusCodes.EnteredInError => ImmunizationStatus.EnteredInError,
                _ => throw new ConceptNotMappedException(
                    $"Could not map {nameof(R4.Immunization.ImmunizationStatusCodes)} to {nameof(ImmunizationStatus)}")
            };
        }
    }
}