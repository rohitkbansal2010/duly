// <copyright file="PatientTargetDetailsConverter.cs" company="Duly Health and Care">
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
    /// Maps <see cref="AdapterModels.CarePlan.CarePlanDetails"/> into <see cref="PatientTargetDetails"/>.
    /// </summary>
    public class PatientTargetDetailsConverter : ITypeConverter<AdapterModels.CarePlan.CarePlanDetails, PatientTargetDetails>
    {
        PatientTargetDetails ITypeConverter<AdapterModels.CarePlan.CarePlanDetails, PatientTargetDetails>.Convert(AdapterModels.CarePlan.CarePlanDetails source, PatientTargetDetails destination, ResolutionContext context)
        {
            return new PatientTargetDetails
            {
                PatientTargetId = source.PatientTargetId,
                TargetName = source.TargetName,
                CustomTargetId = source.CustomTargetId,
                CustomTarget = source.CustomTarget,
                PatientActionDetails = context.Mapper.Map<PatientActionDetails>(source)
            };
        }
    }
}