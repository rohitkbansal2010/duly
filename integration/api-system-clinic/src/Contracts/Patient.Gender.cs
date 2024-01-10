// <copyright file="Patient.Gender.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Describes a birth gender of the patient")]
    public enum Gender
    {
        [Description("Female gender")]
        Female,
        [Description("Male gender")]
        Male,
        [Description("Other gender")]
        Other,
        [Description("A proper value is applicable, but not known.")]
        Unknown
    }
}
