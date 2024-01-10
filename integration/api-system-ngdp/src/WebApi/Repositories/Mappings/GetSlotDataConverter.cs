// <copyright file="GetSlotDataConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Api.Repositories.Mappings.Converters;
using Duly.Ngdp.Contracts;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps <see cref="AdapterModels.DepartmentVisitType"/> into <see cref="DepartmentVisitType"/>.
    /// </summary>
    public class GetSlotDataConverter : ITypeConverter<AdapterModels.DepartmentVisitType, DepartmentVisitType>
    {
        DepartmentVisitType ITypeConverter<AdapterModels.DepartmentVisitType, DepartmentVisitType>.Convert(AdapterModels.DepartmentVisitType source, DepartmentVisitType destination, ResolutionContext context)
        {
            return new DepartmentVisitType
            {
                DepartmentId = source.DepartmentId,
                VisitTypeId = source.VisitTypeId,
                ProviderId = source.ProviderId,
                PatientId = source.PatientId
            };
        }
    }
}