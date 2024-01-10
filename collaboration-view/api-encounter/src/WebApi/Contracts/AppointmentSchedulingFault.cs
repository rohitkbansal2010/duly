// -----------------------------------------------------------------------
// <copyright file="AppointmentSchedulingFault.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.Common.Annotations.Json;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Describes the reason of appointment scheduling failure.")]
    public class AppointmentSchedulingFault
    {
        [Description("Reason for the appointment scheduling failure.")]
        [Required]
        public AppointmentSchedulingFaultReason Reason { get; init; }

        [Description("Appointment scheduling failure error message.")]
        [Required]
        public string ErrorMessage { get; init; }
    }
}