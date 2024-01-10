// <copyright file="AllergyIntoleranceConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using AutoMapper;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Extensions.Fhir;
using System.Collections.Generic;
using System.Linq;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps http://hl7.org/fhir/StructureDefinition/AllergyIntolerance
    /// into AllergyIntolerance.
    /// </summary>
    public class AllergyIntoleranceConverter : ITypeConverter<R4.AllergyIntolerance, AllergyIntolerance>
    {
        public AllergyIntolerance Convert(R4.AllergyIntolerance source, AllergyIntolerance destination, ResolutionContext context)
        {
            return new AllergyIntolerance
            {
                RecordedDate = source.RecordedDateElement.BuildDateTimeOffset(),
                Id = source.Id,
                Reactions = ConvertReactions(source.Reaction, context),
                AllergenName = source.Code.Text,
                Categories = ConvertCategories(source.Category, context),
            };
        }

        private static AllergyIntoleranceCategory[] ConvertCategories(IEnumerable<R4.AllergyIntolerance.AllergyIntoleranceCategory?> categories, ResolutionContext context)
        {
            if (categories is null || !categories.Any())
            {
                return new[] { AllergyIntoleranceCategory.Other };
            }

            return context.Mapper.Map<AllergyIntoleranceCategory[]>(categories);
        }

        private static AllergyIntoleranceReaction[] ConvertReactions(List<R4.AllergyIntolerance.ReactionComponent> reactions, ResolutionContext context)
        {
            return context.Mapper.Map<AllergyIntoleranceReaction[]>(reactions);
        }
    }
}