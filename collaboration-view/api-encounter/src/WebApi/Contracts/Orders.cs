// -----------------------------------------------------------------------
// <copyright file="Orders.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Data of the test orders")]
    public class Orders
    {
        [Description("Names of the lab orders")]
        public string OrderName { get; set; }

        [Description("Authored On")]
        public string AuthoredOn { get; set; }
    }
}