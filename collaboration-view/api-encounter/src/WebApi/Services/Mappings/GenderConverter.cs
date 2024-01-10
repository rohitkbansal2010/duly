// <copyright file="GenderConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    internal class GenderConverter : ITypeConverter<Models.Gender, Gender>
    {
        public Gender Convert(Models.Gender source, Gender destination, ResolutionContext context)
        {
            return source switch
            {
                Models.Gender.Female => Gender.Female,
                Models.Gender.Male => Gender.Male,
                Models.Gender.Other => Gender.Other,
                Models.Gender.Unknown => Gender.Unknown,
                _ => throw new ServiceNotMappedException("Could not map Gender to contract")
            };
        }
    }
}