// <copyright file="CheckOutDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut
{
    public class CheckOutDetails
    {
        /// <summary>
        /// list of labdetails and Imaging.
        /// </summary>
        public IEnumerable<GetLabOrImaging> LabDetailsList { get; set; }

        /// <summary>
        /// List of Schedule Follow Up and Refferal.
        /// </summary>
        public IEnumerable<ScheduleReferral> ScheduleFollowUpList { get; set; }

        /// <summary>
        /// Error Message.
        /// </summary>
        public string Message { get; set; }
    }
}