// <copyright file="PhoneNumber.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Annotations.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Patient with details")]
    public class PhoneNumber
    {
        [Description("Phone number of the patient")]
        public string PhoneNum { get; set; }
        [Description("use")]
        public string Use { get; set; }
    }
}
