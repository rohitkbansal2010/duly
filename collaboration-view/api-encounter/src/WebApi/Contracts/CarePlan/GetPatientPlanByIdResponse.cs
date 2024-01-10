// <copyright file="GetPatientPlanByIdResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class GetPatientPlanByIdResponse
    {
        public GetPatientPlanByPatientId PatientPlans { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}