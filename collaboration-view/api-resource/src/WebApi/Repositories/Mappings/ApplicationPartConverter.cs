// <copyright file="ApplicationPartConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using AdapterContracts = Duly.UiConfig.Adapter.Contracts;

namespace Duly.CollaborationView.Resource.Api.Repositories.Mappings
{
    public class ApplicationPartConverter : ITypeConverter<Contracts.ApplicationPart, AdapterContracts.ApplicationPart>
    {
        public AdapterContracts.ApplicationPart Convert(Contracts.ApplicationPart source, AdapterContracts.ApplicationPart destination, ResolutionContext context)
        {
            return source switch
            {
                Contracts.ApplicationPart.Calendar => AdapterContracts.ApplicationPart.Calendar,
                Contracts.ApplicationPart.CurrentAppointment => AdapterContracts.ApplicationPart.CurrentAppointment,
                _ => throw new RepositoryNotMappedException("Could not map ApplicationPart to corresponding adapter contract.")
            };
        }
    }
}