// <copyright file="Medications.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Duly.Clinic.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class MedicationsExampleProvider : IExamplesProvider<IEnumerable<Medication>>
    {
        public IEnumerable<Medication> GetExamples()
        {
            var provider = new PractitionerGeneralInfoExampleProvider();
            yield return new Medication
            {
                Id = Guid.NewGuid().ToString(),
                Drug = new Drug
                {
                    Title = "Spriva© HandiHaler© 18 mcg"
                },
                Prescriber = provider.GetExamples(),
                Status = MedicationStatus.Active,
                Period = new Period
                {
                    Start = new DateTimeOffset(2012, 11, 21, 0, 0, 0, default)
                },
                Dosages = new[]
                {
                    new Dosage
                    {
                        AsNeeded = false,
                        PatientInstruction = "Inhale 1 puff by mouth 2 times a day",
                        Text = "Inhale 1 puff by mouth 2 times a day",
                        DoseQuantity = new Quantity
                        {
                            Unit = "puff",
                            Value = 1
                        },
                        Timing = new Timing
                        {
                            Repeat = new Repeat
                            {
                                BoundsPeriod = new Period
                                {
                                    Start = new DateTimeOffset(2012, 11, 21, 0, 0, 0, default),
                                    End = new DateTimeOffset(2022, 11, 21, 0, 0, 0, default),
                                },
                                DaysOfWeek = new[] { DaysOfWeek.Mon },
                                PeriodUnit = UnitsOfTime.Wk,
                                TimesOfDay = new[] { "morning" },
                                When = new[] { EventTiming.AC }
                            }
                        }
                    }
                },
                Reason = new MedicationReason
                {
                    ReasonText = new[] { "For chronic lung disease (COPD)" }
                }
            };
            yield return new Medication
            {
                Id = Guid.NewGuid().ToString(),
                Drug = new Drug
                {
                    Title = "Lasix Tablets 40 mg"
                },
                Status = MedicationStatus.Inactive,
                Period = new Period
                {
                    End = new DateTimeOffset(2022, 11, 21, 0, 0, 0, default),
                    Start = new DateTimeOffset(2012, 11, 21, 0, 0, 0, default)
                },
                Dosages = new[]
                {
                    new Dosage
                    {
                        AsNeeded = false,
                        PatientInstruction = "Inhale 1 puff by mouth 2 times a day",
                        Text = "Inhale 1 puff by mouth 2 times a day",
                        DoseQuantity = new Quantity
                        {
                            Unit = "puff",
                            Value = 1
                        },
                        Timing = new Timing
                        {
                            Repeat = new Repeat
                            {
                                BoundsPeriod = new Period
                                {
                                    Start = new DateTimeOffset(2012, 11, 21, 0, 0, 0, default),
                                    End = new DateTimeOffset(2022, 11, 21, 0, 0, 0, default),
                                },
                                DaysOfWeek = new[] { DaysOfWeek.Sat },
                                PeriodUnit = UnitsOfTime.Mo,
                                TimesOfDay = new[] { "morning" },
                                When = new[] { EventTiming.AFT }
                            }
                        }
                    },
                    new Dosage
                    {
                        AsNeeded = false,
                        PatientInstruction = "Inhale 1 puff by mouth 2 times a day",
                        Text = "Inhale 1 puff by mouth 2 times a day",
                        DoseQuantity = new Quantity
                        {
                            Unit = "puff",
                            Value = 1
                        },
                        Timing = new Timing
                        {
                            Repeat = new Repeat
                            {
                                BoundsPeriod = new Period
                                {
                                    Start = new DateTimeOffset(2012, 11, 21, 0, 0, 0, default),
                                    End = new DateTimeOffset(2022, 11, 21, 0, 0, 0, default),
                                },
                                DaysOfWeek = new[] { DaysOfWeek.Wed },
                                PeriodUnit = UnitsOfTime.H,
                                TimesOfDay = new[] { "morning" },
                                When = new[] { EventTiming.WAKE }
                            }
                        }
                    }
                },
                Reason = new MedicationReason
                {
                    ReasonText = new[] { "For Leg swelling (COPD)" }
                }
            };
        }
    }
}