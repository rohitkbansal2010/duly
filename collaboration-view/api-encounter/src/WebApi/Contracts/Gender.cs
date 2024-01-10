// <copyright file="Gender.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("The gender of a person used for administrative purposes")]
    public enum Gender
    {
        [Description("Male")]
        Male,

        [Description("Female")]
        Female,

        [Description("Other")]
        Other,

        [Description("Unknown")]
        Unknown
    }
}