// <copyright file="DepartmentVisitType.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("Department and Visit Type Id")]
    public class DepartmentVisitType
    {
        [Description("Department ID")]
        public string DepartmentId { get; set; }

        [Description("Visit Type Id")]
        public string VisitTypeId { get; set; }

        [Description("Provider Id")]
        public string ProviderId { get; set; }

        [Description("Patient Id")]
        public string PatientId { get; set; }
    }
}