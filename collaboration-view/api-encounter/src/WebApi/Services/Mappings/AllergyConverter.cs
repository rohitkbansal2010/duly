// <copyright file="AllergyConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Microsoft.OpenApi.Extensions;
using System.Linq;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    internal class AllergyConverter : ITypeConverter<Models.AllergyIntolerance, Allergy>
    {
        public Allergy Convert(
            Models.AllergyIntolerance source,
            Allergy destination,
            ResolutionContext context)
        {
            var categories = context.Mapper.Map<AllergyCategory[]>(source.Categories);
            var reactions = context.Mapper.Map<AllergyReaction[]>(source.Reactions);

            return new()
            {
                Id = source.Id,
                Title = source.AllergenName,
                Categories = categories.OrderBy(x => x.GetDisplayName()).ToArray(),
                Recorded = source.RecordedDate,
                Reactions = reactions.OrderBy(x => x.Title).ToArray(),
            };
        }
    }
}
