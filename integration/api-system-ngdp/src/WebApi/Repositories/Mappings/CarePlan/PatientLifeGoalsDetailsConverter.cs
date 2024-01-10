// <copyright file="PatientLifeGoalsDetailsConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Api.Repositories.Mappings.Converters;
using Duly.Ngdp.Contracts;
using Duly.Ngdp.Contracts.CarePlan;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps <see cref="AdapterModels.CarePlan.CarePlanDetails"/> into <see cref="PatientLifeGoalsDetails"/>.
    /// </summary>
    public class PatientLifeGoalsDetailsConverter : ITypeConverter<AdapterModels.CarePlan.CarePlanDetails, PatientLifeGoalsDetails>
    {
        PatientLifeGoalsDetails ITypeConverter<AdapterModels.CarePlan.CarePlanDetails, PatientLifeGoalsDetails>.Convert(AdapterModels.CarePlan.CarePlanDetails source, PatientLifeGoalsDetails destination, ResolutionContext context)
        {
            return new PatientLifeGoalsDetails
            {
                PatientLifeGoalId = source.PatientLifeGoalId,
                LifeGoalDescription = source.LifeGoalDescription,
                LifeGoalName = source.LifeGoalName
            };
        }
    }
}