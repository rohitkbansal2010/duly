// <copyright file="AdapterApplicationPartConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using AdapterContracts = Duly.UiConfig.Adapter.Contracts;

namespace Duly.CollaborationView.Resource.Api.Repositories.Mappings
{
    public class AdapterApplicationPartConverter : ITypeConverter<AdapterContracts.ApplicationPart, Contracts.ApplicationPart>
    {
        public Contracts.ApplicationPart Convert(AdapterContracts.ApplicationPart source, Contracts.ApplicationPart destination, ResolutionContext context)
        {
            return source switch
            {
                AdapterContracts.ApplicationPart.Calendar => Contracts.ApplicationPart.Calendar,
                AdapterContracts.ApplicationPart.CurrentAppointment => Contracts.ApplicationPart.CurrentAppointment,
                _ => throw new RepositoryNotMappedException("Could not map ApplicationPart to corresponding contract.")
            };
        }
    }
}