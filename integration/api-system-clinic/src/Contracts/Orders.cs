// <copyright file="Orders.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Data of the test orders")]
    public class Orders
    {
        [Description("Names of the lab orders")]
        public string OrderName { get; set; }

        [Description("Ordered Date")]
        public string AuthoredOn { get; set; }
    }
}