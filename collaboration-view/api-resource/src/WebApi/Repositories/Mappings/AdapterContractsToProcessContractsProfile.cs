// <copyright file="AdapterContractsToProcessContractsProfile.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using AdapterContracts = Duly.UiConfig.Adapter.Contracts;

namespace Duly.CollaborationView.Resource.Api.Repositories.Mappings
{
    /// <summary>
    /// Holds mappings for conversion from adapter contracts to process level contracts.
    /// </summary>
    internal class AdapterContractsToProcessContractsProfile : Profile
    {
        public AdapterContractsToProcessContractsProfile()
        {
            CreateMap<AdapterContracts.ApplicationPart, Contracts.ApplicationPart>()
                .ConvertUsing<AdapterApplicationPartConverter>();
            CreateMap<AdapterContracts.UiConfigurationTargetAreaType, Contracts.UiConfigurationTargetAreaType>()
                .ConvertUsing<AdapterUiConfigurationTargetAreaTypeConverter>();

            CreateMap<AdapterContracts.UiConfigurationWithChildren, Contracts.UiConfiguration>()
                .ConvertUsing<AdapterUiConfigurationWithChildrenConverter>();
        }
    }
}