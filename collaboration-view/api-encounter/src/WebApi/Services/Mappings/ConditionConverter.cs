// <copyright file="ConditionConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    internal class ConditionConverter : ITypeConverter<Models.Condition, HealthCondition>
    {
        public HealthCondition Convert(Models.Condition source, HealthCondition destination, ResolutionContext context)
        {
            var condition = new HealthCondition
            {
                Title = source.Name,
                Date = source.ClinicalStatus == Models.ConditionClinicalStatus.Resolved
                    ? source.AbatementPeriod?.End
                    : source.RecordedDate
            };

            return condition;
        }
    }
}
