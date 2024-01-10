// <copyright file="PatientPhoto.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Patient Photo")]

    public class PatientPhoto
    {
        [Description("Photo")]
        public List<string> Photo { get; set; }

        [Description("Status Code")]
        public string StatusCode { get; set; }
    }
}
