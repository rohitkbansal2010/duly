// <copyright file="ParticipantToPartyConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    internal class ParticipantToPartyConverter : ITypeConverter<Models.Participant, Party>
    {
        public Party Convert(Models.Participant source, Party destination, ResolutionContext context)
        {
            return source.MemberType switch
            {
                Models.MemberType.Practitioner => context.Mapper.Map<Party>(source.Person as Models.PractitionerGeneralInfo),
                _ => throw new ServiceNotMappedException($"Unmapped MemberType {source.MemberType}"),
            };
        }
    }
}