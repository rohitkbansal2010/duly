// <copyright file="PhoneNumber.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Address")]

    public class PhoneNumber
    {
        [Description("Phone Number")]
        public string PhoneNum { get; set; }
        [Description("use")]
        public string Use { get; set; }
    }
}
