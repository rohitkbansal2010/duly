// <copyright file="Party.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts.Extensions;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class PartiesExampleProvider : IExamplesProvider<IEnumerable<Party>>
    {
        public IEnumerable<Party> GetExamples()
        {
            yield return new Party
            {
                Id = Guid.NewGuid().ToString(),
                HumanName = new HumanName
                {
                    FamilyName = "Ling",
                    GivenNames = new[] { "Nuha", "Malikah" },
                    Prefixes = new[] { "Dr" }
                },
                MemberType = MemberType.Doctor
            };
            yield return new Party
            {
                Id = Guid.NewGuid().ToString(),
                HumanName = new HumanName
                {
                    FamilyName = "Sussman",
                    GivenNames = new[] { "Judith" },
                    Prefixes = new[] { "Dr" }
                },
                Photo = PhotoExample.MakeAttachment(PhotoExample.Photo.Photo1),
                MemberType = MemberType.Doctor
            };
            yield return new Party
            {
                Id = Guid.NewGuid().ToString(),
                HumanName = new HumanName
                {
                    FamilyName = "Valdez",
                    GivenNames = new[] { "Cozart" },
                },
                Photo = PhotoExample.MakeAttachment(PhotoExample.Photo.Photo2),
                MemberType = MemberType.Caregiver
            };
            yield return new Party
            {
                Id = Guid.NewGuid().ToString(),
                HumanName = new HumanName
                {
                    FamilyName = "Mckay",
                    GivenNames = new[] { "Palmero", "Anika" },
                },
                MemberType = MemberType.Another
            };
        }
    }
}
