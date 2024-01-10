// -----------------------------------------------------------------------
// <copyright file="CheckOutDetails.cs" company="Duly Health and Care">
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
    [Description("Lab Details of the Patient")]
    public class CheckOutDetails
    {
        [Description("List of labs and Imaging")]
        public IEnumerable<GetLabOrImaging> LabDetailsList { get; set; }
        [Description("list of schedule or followUp")]
        public IEnumerable<ScheduleReferral> ScheduleFollowUpList { get; set; }
        [Description("Error Message")]
        public string Message { get; set; }
    }
}