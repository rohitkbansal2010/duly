// <copyright file="Party.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Party which contain information about participant of the event")]
    public class Party
    {
        [Description("Id of the participant")]
        [Required]
        public string Id { get; set; }

        [Description("Name of the participant with parts and usage")]
        [Required]
        public HumanName HumanName { get; set; }

        [Description("Image of the participant")]
        public Attachment Photo { get; set; }

        [Description("Type of the participant")]
        [Required]
        public MemberType MemberType { get; set; }

        [Description("Role of a Practitioner")]
        public PractitionerRole Role { get; set; }

        [Description("Speciality of a Practitioner")]
        public List<string> Speciality { get; set; }
    }
}