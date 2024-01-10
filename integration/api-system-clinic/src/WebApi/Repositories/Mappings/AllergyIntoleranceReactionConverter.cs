// <copyright file="AllergyIntoleranceReactionConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using AutoMapper;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps reaction (BackboneElement) of http://hl7.org/fhir/StructureDefinition/AllergyIntolerance
    /// into AllergyIntoleranceReaction.
    /// </summary>
    public class AllergyIntoleranceReactionConverter : ITypeConverter<R4.AllergyIntolerance.ReactionComponent,
        Contracts.AllergyIntoleranceReaction>
    {
        public Contracts.AllergyIntoleranceReaction Convert(
            R4.AllergyIntolerance.ReactionComponent source,
            Contracts.AllergyIntoleranceReaction destination,
            ResolutionContext context)
        {
            return new Contracts.AllergyIntoleranceReaction
            {
                Title = source.Description,
                Severity = context.Mapper.Map<Contracts.AllergyIntoleranceReactionSeverity>(source.Severity)
            };
        }
    }
}