// <copyright file="AdapterUiConfigurationTargetAreaTypeConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using AdapterContracts = Duly.UiConfig.Adapter.Contracts;
using UiConfigurationTargetAreaType = Duly.CollaborationView.Resource.Api.Contracts.UiConfigurationTargetAreaType;

namespace Duly.CollaborationView.Resource.Api.Repositories.Mappings
{
    internal class AdapterUiConfigurationTargetAreaTypeConverter : ITypeConverter<AdapterContracts.UiConfigurationTargetAreaType, UiConfigurationTargetAreaType>
    {
        public UiConfigurationTargetAreaType Convert(AdapterContracts.UiConfigurationTargetAreaType source, UiConfigurationTargetAreaType destination, ResolutionContext context)
        {
            return source switch
            {
                AdapterContracts.UiConfigurationTargetAreaType.Layout => UiConfigurationTargetAreaType.Layout,
                AdapterContracts.UiConfigurationTargetAreaType.Navigation => UiConfigurationTargetAreaType.Navigation,
                _ => throw new RepositoryNotMappedException("Could not map UiConfigurationTargetAreaType to corresponding contract.")
            };
        }
    }
}