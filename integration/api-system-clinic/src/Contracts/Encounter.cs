// <copyright file="Encounter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts.Interfaces;
using Duly.Common.Security.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("An interaction during which services are provided to the patient")]
    public class Encounter : IDulyResource
    {
        [Description("Identifier by which this encounter is known")]
        [Required]
        [Identity]
        public string Id { get; set; }

        [Description("Specific type of service")]
        [Required]
        public string ServiceType { get; set; }

        [Description("Specific type of encounter")]
        public EncounterType Type { get; set; }

        [Description("Status of the visit")]
        public EncounterStatus Status { get; set; }

        [Description("When appointment is to take place")]
        [Required]
        public TimeSlot TimeSlot { get; set; }

        [Description("Where appointment is to take place")]
        public Location Location { get; set; }

        [Description("Participant of the meeting from patient side")]
        [Required]
        public PatientGeneralInfoWithVisitsHistory Patient { get; set; }

        [Description("Participant of the meeting from practitioner side")]
        [Required]
        public PractitionerGeneralInfo Practitioner { get; set; }
    }
}
