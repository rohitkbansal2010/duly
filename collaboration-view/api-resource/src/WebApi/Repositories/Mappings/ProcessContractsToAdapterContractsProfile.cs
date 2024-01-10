// <copyright file="ProcessContractsToAdapterContractsProfile.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using AdapterContracts = Duly.UiConfig.Adapter.Contracts;

namespace Duly.CollaborationView.Resource.Api.Repositories.Mappings
{
    /// <summary>
    /// Holds mappings for conversion from process level contracts to adapter contracts.
    /// </summary>
    internal class ProcessContractsToAdapterContractsProfile : Profile
    {
        public ProcessContractsToAdapterContractsProfile()
        {
            CreateMap<Contracts.ApplicationPart, AdapterContracts.ApplicationPart>()
                .ConvertUsing<ApplicationPartConverter>();
        }
    }
}