// <copyright file="Participant.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Participant involved in the encounter")]
    public class Participant
    {
        [Description("Participant's type involved in the encounter other than the patient")]
        [Required]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "Requirements")]
        public MemberType MemberType
        {
            get
            {
                return Person switch
                {
                    PractitionerGeneralInfo => MemberType.Practitioner,
                    RelatedPersonGeneralInfo => MemberType.RelatedPerson,
                    _ => throw new ArgumentOutOfRangeException($"Person type ['{Person.GetType()}'] is not supported")
                };
            }
        }

        [Description("Human specific information about person")]
        [Required]
        public HumanGeneralInfoWithPhoto Person { get; set; }
    }
}