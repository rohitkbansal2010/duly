// <copyright file="ReferralAppointment.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Duly.Ngdp.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "ExampleProviders")]
    public class ReferralAppointmentExampleProvider : IExamplesProvider<IEnumerable<ReferralAppointment>>
    {
        public IEnumerable<ReferralAppointment> GetExamples()
        {
            yield return new ReferralAppointment
            {
                ReferralId = "16620533",
                ScheduledTime = DateTime.UtcNow,
                Visit = new Visit
                {
                    Type = new VisitType
                    {
                        Identifier = new Identifier
                        {
                            Id = "5507",
                            Type = IdentifierType.External
                        }
                    }
                },
                Location = new Location
                {
                    Name = "ELMHURST-DMG",
                    Address = BuildElginAddress(),
                },
                Department = new Department
                {
                    Identifier = new Identifier
                    {
                        Id = "23231",
                        Type = IdentifierType.External
                    },
                    Name = "Cardiology - Airlite St, Elgin"
                },
                Appointment = new ScheduledAppointment
                {
                    DateTime = DateTime.UtcNow.AddDays(-5),
                    DurationInMinutes = 30,
                    TimeZone = "CT"
                },
                Provider = new ScheduledProvider
                {
                    Identifier = new Identifier
                    {
                        Id = "112368",
                        Type = IdentifierType.External
                    },
                    Name = "Syed Hasan, MD",
                    PhotoURL = @"https://npd.dupagemedicalgroup.com:8444/assets/oam3VAyAIqWVURVvPb4KKllh1U8fLv2Bxrg7TkxJhC8/gravity:sm/resize:fill:800:800:1:1/aHR0cHM6Ly9kbWd3ZWJ0ZXN0c3RvcmFnZS5ibG9iLmNvcmUud2luZG93cy5uZXQvZG1ndGVzdHdlYi9waHlzaWNpYW4taGVhZHNob3RzL1JlaWxseV9Kb2huX09ydGhvcGFlZGljcy0wMDFfd2ViLkpQRw==.webp"
                }
            };
        }

        private static Address BuildElginAddress()
        {
            return new Address
            {
                City = "ELGIN",
                State = "IL",
                Lines = new[]
                {
                    "87 N AIRLITE ST"
                }
            };
        }
    }
}