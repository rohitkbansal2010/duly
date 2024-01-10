// <copyright file="HumanGeneralInfoWithPhoto.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts.Abstractions;
using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Abstraction of a human name with photo")]
    public class HumanGeneralInfoWithPhoto : HumanGeneralInfo
    {
        [Description("Images of the human.")]
        public Attachment[] Photos { get; set; }
    }
}