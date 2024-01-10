// <copyright file="CheckOutDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("Lab Details of the Patient")]
    public class CheckOutDetails
    {
        [Description("List of labs and Imaging")]
        public IEnumerable<GetLabOrImagingDetails> LabDetailsList { get; set; }
        [Description("list of schedule or followUp")]
        public IEnumerable<ReferralDetail> ScheduleFollowUpList { get; set; }
        [Description("Error Message")]
        public string Message { get; set; }
    }
}