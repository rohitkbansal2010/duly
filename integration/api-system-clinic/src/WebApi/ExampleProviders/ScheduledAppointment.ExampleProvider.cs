// <copyright file="ScheduledAppointment.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Duly.Clinic.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class ScheduledAppointmentExampleProvider : IExamplesProvider<ScheduledAppointment>
    {
        public ScheduledAppointment GetExamples()
        {
            return new ()
            {
                Date = DateTime.Now.AddDays(5),
                DurationInMinutes = 15,
                Time = TimeSpan.Parse("14:15:00"),
                Patient = new ScheduledPatientExampleProvider().GetExamples(),
                Provider = new ScheduledProviderExampleProvider().GetExamples(),
                Department = new ScheduledDepartmentExampleProvider().GetExamples(),
                VisitType = new ScheduledVisitTypeExampleProvider().GetExamples(),
                PatientInstructions = new[]
                {
                    "Hello,",
                    "<br>",
                    "<br>To keep our patients, visitors and care team members safe and healthy at this time of year when illness is at its peak, Duly Health and Care is working to ensure physical distancing can occur throughout our care environment. ",
                    "<br>",
                    "<br>We ask that you bring no more than one companion, over the age of 18, to support your care visit. If space is limited, we may ask that individual to wait in the car, assured that our care team will provider timely updates and instructions for when they can enter the clinic.",
                    "<br>",
                    "<br>For pediatrics, please bring only those children who are seeing a provider that day. ",
                    "<br>",
                    "<br>If you have recently tested positive for COVID-19, have been exposed to someone with a confirmed case or are experiencing symptoms, please call the clinic before coming in. ",
                    "<br>",
                    "<br>Duly Health and Care",
                    "<br>If you are signed up for MyChart, you will be receiving a questionnaire for this visit.  Please know, it is important that you complete this online prior to the visit."
                },
                ContactIds = new[]
                {
                    "DAT|55338", "ASN|700016788", "CSN|700016788", "UCI|"
                },
                ScheduledTime = DateTime.UtcNow
            };
        }
    }
}
