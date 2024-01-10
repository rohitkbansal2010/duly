// <copyright file="Appointment.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class AppointmentsExampleProvider : IExamplesProvider<IEnumerable<Appointment>>
    {
        public IEnumerable<Appointment> GetExamples()
        {
            yield return new Appointment
            {
                Id = "qwerty1",
                Title = "Wellness Check",
                Status = AppointmentStatus.Completed,
                Type = AppointmentType.OnSite,
                TimeSlot = new TimeSlot
                {
                    StartTime = new DateTime(2021, 12, 23, 10, 30, 0),
                    EndTime = new DateTime(2021, 12, 23, 11, 0, 0)
                },
                Location = new Location
                {
                    Title = "Room #5"
                },
                Patient = new PatientExtendedInfo
                {
                    PatientGeneralInfo = new PatientGeneralInfo
                    {
                        Id = "qwerty1",
                        HumanName = new HumanName
                        {
                            FamilyName = "Garcia",
                            GivenNames = new[] { "Lucas" }
                        }
                    },
                    IsNewPatient = true,
                },
                Practitioner = new PractitionerGeneralInfo
                {
                    Id = "qwerty1",
                    HumanName = new HumanName
                    {
                        FamilyName = "Ling",
                        Prefixes = new[] { "Dr." }
                    },
                    Role = PractitionerRole.PrimaryCarePhysician
                }
            };
            yield return new Appointment
            {
                Id = "qwerty2",
                Title = "Check Up",
                Status = AppointmentStatus.Arrived,
                Type = AppointmentType.Telehealth,
                TimeSlot = new TimeSlot
                {
                    StartTime = new DateTime(2021, 12, 23, 11, 00, 0),
                    EndTime = new DateTime(2021, 12, 23, 11, 15, 0)
                },
                Location = new Location
                {
                    Title = "Room #3"
                },
                Patient = new PatientExtendedInfo
                {
                    PatientGeneralInfo = new PatientGeneralInfo
                    {
                        Id = "qwerty2",
                        HumanName = new HumanName
                        {
                            FamilyName = "Smith",
                            GivenNames = new[] { "Benjamin" }
                        }
                    },
                    IsNewPatient = false,
                },
                Practitioner = new PractitionerGeneralInfo
                {
                    Id = "qwerty2",
                    HumanName = new HumanName
                    {
                        FamilyName = "Sussman",
                        Prefixes = new[] { "Dr." }
                    },
                    Role = PractitionerRole.PrimaryCarePhysician
                }
            };
            yield return new Appointment
            {
                Id = "qwerty3",
                Title = "Check Up",
                Status = AppointmentStatus.InProgress,
                Type = AppointmentType.OnSite,
                TimeSlot = new TimeSlot
                {
                    StartTime = new DateTime(2021, 12, 23, 11, 0, 0),
                    EndTime = new DateTime(2021, 12, 23, 11, 30, 0)
                },
                Location = new Location
                {
                    Title = "Room #2"
                },
                Patient = new PatientExtendedInfo
                {
                    PatientGeneralInfo = new PatientGeneralInfo
                    {
                        Id = "qwerty3",
                        HumanName = new HumanName
                        {
                            FamilyName = "Johnson",
                            GivenNames = new[] { "Mary" }
                        }
                    },
                    IsNewPatient = true,
                },
                Practitioner = new PractitionerGeneralInfo
                {
                    Id = "qwerty3",
                    HumanName = new HumanName
                    {
                        FamilyName = "Palmero"
                    },
                    Role = PractitionerRole.MedicalAssistant
                }
            };
        }
    }
}
