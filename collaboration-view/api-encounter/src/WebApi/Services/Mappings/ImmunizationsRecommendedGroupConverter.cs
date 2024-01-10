// <copyright file="ImmunizationsRecommendedGroupConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    internal class ImmunizationsRecommendedGroupConverter : ITypeConverter<RecommendedImmunization, ImmunizationsRecommendedGroup>
    {
        public const string Duetitle = "DUE";

        public ImmunizationsRecommendedGroup Convert(RecommendedImmunization source, ImmunizationsRecommendedGroup destination, ResolutionContext context)
        {
            return new ImmunizationsRecommendedGroup
            {
                Title = source.VaccineName,
                Vaccinations = new RecommendedVaccination[]
                {
                    new()
                    {
                        Date = source.DueDate,
                        DateTitle = Duetitle,
                        Status = context.Mapper.Map<RecommendedVaccinationStatus>(source.Status)
                    }
                }
            };
        }
    }
}