// <copyright file="PatientActionDetailsConverter.cs" company="Duly Health and Care">
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
    /// Maps <see cref="AdapterModels.CarePlan.CarePlanDetails"/> into <see cref="PatientActionDetails"/>.
    /// </summary>
    public class PatientActionDetailsConverter : ITypeConverter<AdapterModels.CarePlan.CarePlanDetails, PatientActionDetails>
    {
        PatientActionDetails ITypeConverter<AdapterModels.CarePlan.CarePlanDetails, PatientActionDetails>.Convert(AdapterModels.CarePlan.CarePlanDetails source, PatientActionDetails destination, ResolutionContext context)
        {
            return new PatientActionDetails
            {
                PatientActionId = source.PatientActionId,
                ActionName = source.ActionName,
                CustomAction = source.CustomAction,
                CustomActionId = source.CustomActionId
            };
        }
    }
}