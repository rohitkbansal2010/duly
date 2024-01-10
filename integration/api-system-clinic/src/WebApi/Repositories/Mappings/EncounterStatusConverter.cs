// <copyright file="EncounterStatusConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using AutoMapper;
using Duly.Clinic.Contracts;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    public class EncounterStatusConverter : ITypeConverter<R4.Encounter.EncounterStatus, EncounterStatus>
    {
        public EncounterStatus Convert(R4.Encounter.EncounterStatus source, EncounterStatus destination, ResolutionContext context)
        {
            return source switch
            {
                R4.Encounter.EncounterStatus.Arrived => EncounterStatus.Arrived,
                R4.Encounter.EncounterStatus.InProgress => EncounterStatus.InProgress,
                R4.Encounter.EncounterStatus.Planned => EncounterStatus.Planned,
                R4.Encounter.EncounterStatus.Cancelled => EncounterStatus.Cancelled,
                R4.Encounter.EncounterStatus.Finished => EncounterStatus.Finished,
                _ => throw new ConceptNotMappedException("Could not map EncounterStatus to System model")
            };
        }
    }
}