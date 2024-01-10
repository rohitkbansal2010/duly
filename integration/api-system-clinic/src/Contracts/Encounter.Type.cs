// <copyright file="Encounter.Type.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Describes a type of a meeting")]
    public enum EncounterType
    {
        [Description("An on-site meeting ")]
        OnSite,
        [Description("A Telehealth meeting")]
        Telehealth
    }
}
