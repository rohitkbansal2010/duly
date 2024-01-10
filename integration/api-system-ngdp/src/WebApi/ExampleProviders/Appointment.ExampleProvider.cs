// <copyright file="Appointment.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Duly.Ngdp.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "ExampleProviders")]
    public class AppointmentExampleProvider : IExamplesProvider<IEnumerable<Appointment>>
    {
        public IEnumerable<Appointment> GetExamples()
        {
            yield return new Appointment
            {
                Id = "700011430",
                Visit = new()
                {
                    TypeId = "8001",
                    Type = "EST PT OFFICE VISIT",
                    TypeDisplayName = "Established Patient Office Visit"
                },
                Status = AppointmentStatus.Scheduled,
                TimeSlot = new()
                {
                    StartTime = DateTimeOffset.Parse("2022-01-31T08:15:00-06:00"),
                    EndTime = DateTimeOffset.Parse("2022-01-31T08:30:00-06:00")
                },
                Patient = new()
                {
                    Id = "7650074"
                },
                Practitioner = new()
                {
                    Id = "1150",
                    HumanName = new()
                    {
                        FamilyName = "Fitzgerald",
                        GivenNames = new[]
                        {
                            "Michael E"
                        }
                    }
                },
                IsTelehealthVisit = false
            };

            yield return new Appointment
            {
                Id = "700011419",
                Visit = new()
                {
                    TypeId = "8001",
                    Type = "EST PT OFFICE VISIT",
                    TypeDisplayName = "Established Patient Office Visit"
                },
                Status = AppointmentStatus.NoShow,
                TimeSlot = new()
                {
                    StartTime = DateTimeOffset.Parse("2022-01-14T08:15:00-06:00"),
                    EndTime = DateTimeOffset.Parse("2022-01-14T08:30:00-06:00")
                },
                Patient = new()
                {
                    Id = "7650074"
                },
                Practitioner = new()
                {
                    Id = "1150",
                    HumanName = new()
                    {
                        FamilyName = "Fitzgerald",
                        GivenNames = new[]
                        {
                            "Michael E"
                        }
                    }
                },
                IsTelehealthVisit = false
            };

            yield return new Appointment
            {
                Id = "700012485",
                Visit = new()
                {
                    TypeId = "5989",
                    Type = "DMG ORTHO US",
                    TypeDisplayName = "ULTRASOUND INJ"
                },
                Status = AppointmentStatus.Unresolved,
                TimeSlot = new()
                {
                    StartTime = DateTimeOffset.Parse("2022-01-11T14:40:00-06:00"),
                    EndTime = DateTimeOffset.Parse("2022-01-11T14:55:00-06:00")
                },
                Patient = new()
                {
                    Id = "7648513"
                },
                Practitioner = new()
                {
                    Id = "28235",
                    HumanName = new()
                    {
                        FamilyName = "ELM ORTHO US"
                    }
                },
                IsTelehealthVisit = false
            };
        }
    }
}