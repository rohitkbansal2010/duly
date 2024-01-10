// <copyright file="Encounters.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Duly.Clinic.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class EncountersExampleProvider : IExamplesProvider<IEnumerable<Encounter>>
    {
        public IEnumerable<Encounter> GetExamples()
        {
            yield return new Encounter
            {
                Id = "qwerty1",
                ServiceType = "Wellness Check",
                Status = EncounterStatus.Finished,
                Type = EncounterType.OnSite,
                TimeSlot = new TimeSlot
                {
                    StartTime = new DateTime(2021, 12, 23, 10, 30, 0),
                    EndTime = new DateTime(2021, 12, 23, 11, 0, 0)
                },
                Location = new Location
                {
                    Title = "Room #5"
                },
                Patient = new PatientGeneralInfoWithVisitsHistory
                {
                    Patient = new PatientGeneralInfo
                    {
                        Id = "qwerty1",
                        Names = HumanNameExampleProvider.GetNames(HumanNameExampleProvider.Build("Garcia", "Lucas"))
                    }
                },
                Practitioner = new PractitionerGeneralInfo
                {
                    Id = "qwerty1",
                    Names = HumanNameExampleProvider.GetNames(HumanNameExampleProvider.Build("Ling", prefix: "Dr."))
                }
            };
            yield return new Encounter
            {
                Id = "qwerty2",
                ServiceType = "Check Up",
                Status = EncounterStatus.Planned,
                Type = EncounterType.Telehealth,
                TimeSlot = new TimeSlot
                {
                    StartTime = new DateTime(2021, 12, 23, 11, 00, 0),
                    EndTime = new DateTime(2021, 12, 23, 11, 15, 0)
                },
                Location = new Location
                {
                    Title = "Room #3"
                },
                Patient = new PatientGeneralInfoWithVisitsHistory
                {
                    Patient = new PatientGeneralInfo
                    {
                        Id = "qwerty2",
                        Names = HumanNameExampleProvider.GetNames(HumanNameExampleProvider.Build("Smith", "Benjamin"))
                    },
                    HasPastVisits = true
                },
                Practitioner = new PractitionerGeneralInfo
                {
                    Id = "qwerty2",
                    Names = HumanNameExampleProvider.GetNames(HumanNameExampleProvider.Build("Sussman")),
                    Roles = new[] { new Role { Title = "Physician" } }
                }
            };
            yield return new Encounter
            {
                Id = "qwerty3",
                ServiceType = "Check Up",
                Status = EncounterStatus.InProgress,
                Type = EncounterType.OnSite,
                TimeSlot = new TimeSlot
                {
                    StartTime = new DateTime(2021, 12, 23, 11, 0, 0),
                    EndTime = new DateTime(2021, 12, 23, 11, 30, 0)
                },
                Location = new Location
                {
                    Title = "Room #2"
                },
                Patient = new PatientGeneralInfoWithVisitsHistory
                {
                    Patient = new PatientGeneralInfo
                    {
                        Id = "qwerty3",
                        Names = HumanNameExampleProvider.GetNames(HumanNameExampleProvider.Build("Johnson", "Mary"))
                    },
                    HasPastVisits = true
                },
                Practitioner = new PractitionerGeneralInfo
                {
                    Id = "qwerty3",
                    Names = HumanNameExampleProvider.GetNames(HumanNameExampleProvider.Build("Palmero"))
                }
            };
        }
    }
}
