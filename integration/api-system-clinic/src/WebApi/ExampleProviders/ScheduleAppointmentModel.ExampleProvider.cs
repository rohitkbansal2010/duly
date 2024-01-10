// <copyright file="ScheduleAppointmentModel.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Duly.Clinic.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class ScheduleAppointmentModelExampleProvider : IExamplesProvider<ScheduleAppointmentModel>
    {
        public ScheduleAppointmentModel GetExamples()
        {
            return new()
            {
                Date = DateTime.Now.AddDays(5).Date,
                Time = TimeSpan.Parse("14:15:00"),
                PatientId = "EXTERNAL|7650074",
                ProviderId = "External|1405",
                DepartmentId = "External|25069",
                VisitTypeId = "External|2147"
            };
        }
    }
}
