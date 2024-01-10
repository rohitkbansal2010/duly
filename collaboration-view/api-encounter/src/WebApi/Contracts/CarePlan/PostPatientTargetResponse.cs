// <copyright file="PostPatientTargetResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class PostPatientTargetResponse
    {
        public long PatientTargetId { get; set; }
        public string StatusCode { get; set; }
        public string Message { get; set; }
    }
}
