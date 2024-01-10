// <copyright file="Drug.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("A substance used as a medication or in the preparation of medication")]
    public class Drug
    {
        [Description("Title that identify this drug")]
        public string Title { get; set; }
    }
}