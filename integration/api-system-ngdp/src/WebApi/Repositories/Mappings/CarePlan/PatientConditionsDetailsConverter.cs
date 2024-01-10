// <copyright file="PatientConditionsDetailsConverter.cs" company="Duly Health and Care">
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
    /// Maps <see cref="AdapterModels.CarePlan.CarePlanDetails"/> into <see cref="PatientConditionsDetails"/>.
    /// </summary>
    public class PatientConditionsDetailsConverter : ITypeConverter<AdapterModels.CarePlan.CarePlanDetails, PatientConditionsDetails>
    {
        PatientConditionsDetails ITypeConverter<AdapterModels.CarePlan.CarePlanDetails, PatientConditionsDetails>.Convert(AdapterModels.CarePlan.CarePlanDetails source, PatientConditionsDetails destination, ResolutionContext context)
        {
            return new PatientConditionsDetails
            {
                PatientConditionId = source.PatientConditionId,
                ConditionDisplayName = source.ConditionDisplayName,
                PatientTargetDetails = context.Mapper.Map<PatientTargetDetails>(source)
            };
        }
    }
}