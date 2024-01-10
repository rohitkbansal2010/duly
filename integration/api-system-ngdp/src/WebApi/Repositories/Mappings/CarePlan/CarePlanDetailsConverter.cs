// <copyright file="CarePlanDetailsConverter.cs" company="Duly Health and Care">
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
    /// Maps <see cref="AdapterModels.CarePlan.CarePlanDetails"/> into <see cref="CarePlanDetails"/>.
    /// </summary>
    public class CarePlanDetailsConverter : ITypeConverter<AdapterModels.CarePlan.CarePlanDetails, CarePlanDetails>
    {
        CarePlanDetails ITypeConverter<AdapterModels.CarePlan.CarePlanDetails, CarePlanDetails>.Convert(AdapterModels.CarePlan.CarePlanDetails source, CarePlanDetails destination, ResolutionContext context)
        {
            return new CarePlanDetails
            {
                PatientPlanDetails = context.Mapper.Map<PatientPlanDetails>(source),
                PatientLifeGoalsDetails = context.Mapper.Map<PatientLifeGoalsDetails>(source)
            };
        }
    }
}