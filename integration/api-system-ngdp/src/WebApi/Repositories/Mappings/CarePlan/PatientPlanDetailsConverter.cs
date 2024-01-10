// <copyright file="PatientPlanDetailsConverter.cs" company="Duly Health and Care">
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
    /// Maps <see cref="AdapterModels.CarePlan.CarePlanDetails"/> into <see cref="PatientPlanDetails"/>.
    /// </summary>
    public class PatientPlanDetailsConverter : ITypeConverter<AdapterModels.CarePlan.CarePlanDetails, PatientPlanDetails>
    {
        PatientPlanDetails ITypeConverter<AdapterModels.CarePlan.CarePlanDetails, PatientPlanDetails>.Convert(AdapterModels.CarePlan.CarePlanDetails source, PatientPlanDetails destination, ResolutionContext context)
        {
            return new PatientPlanDetails
            {
                AppointmentId = source.AppointmentId,
                PlanName = source.PlanName,
                PatientPlanId = source.PatientPlanId,
                PatientConditionsDetails = context.Mapper.Map<PatientConditionsDetails>(source)
            };
        }
    }
}