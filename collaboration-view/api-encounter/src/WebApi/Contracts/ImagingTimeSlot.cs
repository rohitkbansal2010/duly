// <copyright file="ImagingTimeSlot.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    public class ImagingTimeSlot
    {
        [Description("Department Id and Provider Id array")]
        public List<Departments> DepartmentAndProvider { get; set; }
        [Description("Start date")]
        public DateTime StartDate { get; set; }
        [Description("End date")]
        public DateTime EndDate { get; set; }
  }

    public class Departments
    {
        [Description("Department Id")]
        public string DepartmentId { get; set; }
        [Description("Provider Id")]
        public List<string> ProviderId { get; set; }
    }
}
