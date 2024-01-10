// <copyright file="CheckOutDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Duly.Ngdp.Adapter.Adapters.Models
{
    public class CheckOutDetails
    {
        /// <summary>
        /// list of labdetails and Imaging.
        /// </summary>
        public IEnumerable<GetLabOrImagingDetails> LabDetailsList { get; set; }

        /// <summary>
        /// List of Schedule Follow Up and Refferal.
        /// </summary>
        public IEnumerable<ReferralDetail> ScheduleFollowUpList { get; set; }

        /// <summary>
        /// Error Message.
        /// </summary>
        public string Message { get; set; }
    }
}