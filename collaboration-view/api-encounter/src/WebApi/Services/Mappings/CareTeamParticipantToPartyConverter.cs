// <copyright file="CareTeamParticipantToPartyConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    internal class CareTeamParticipantToPartyConverter :
        ITypeConverter<CareTeamParticipant, Party>,
        ITypeConverter<PractitionerInCareTeam, Party>
    {
        public Party Convert(CareTeamParticipant source, Party destination, ResolutionContext context)
        {
            return source.MemberRole switch
            {
                MemberRole.Practitioner => context.Mapper.Map<Party>(source.Member as PractitionerInCareTeam),
                _ => throw new ServiceNotMappedException($"Unmapped MemberRole {source.MemberRole}"),
            };
        }

        public Party Convert(PractitionerInCareTeam source, Party destination, ResolutionContext context)
        {
            return context.Mapper.Map<Party>(source.Practitioner);
        }
    }
}