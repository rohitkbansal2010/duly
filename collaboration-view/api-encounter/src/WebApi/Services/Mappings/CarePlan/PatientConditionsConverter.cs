// <copyright file="PatientConditionsConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    public class PatientConditionsConverter : ITypeConverter<Contracts.CarePlan.PatientConditions, Models.PatientConditions>
    {
        public PatientConditions Convert(Contracts.CarePlan.PatientConditions source, PatientConditions destination, ResolutionContext context)
        {
            return new Models.PatientConditions
            {
                PatientPlanId = source.PatientPlanId,
                AddConditionIds = source.AddConditionIds,
                RemoveConditionIds = source.RemoveConditionIds
            };
        }
    }
}