// <copyright file="CareTeamParticipantConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps
    ///     http://hl7.org/fhir/StructureDefinition/Practitioners with http://hl7.org/fhir/StructureDefinition/PractitionerRole,
    /// into <see cref="CareTeamParticipant"/>.
    /// </summary>
    public class CareTeamParticipantConverter : ITypeConverter<PractitionerWithRoles, CareTeamParticipant>
    {
        public CareTeamParticipant Convert(PractitionerWithRoles source, CareTeamParticipant destination, ResolutionContext context)
        {
            return new CareTeamParticipant
            {
                Member = new PractitionerInCareTeam
                {
                    Practitioner = context.Mapper.Map<PractitionerGeneralInfo>(source)
                }
            };
        }
    }
}