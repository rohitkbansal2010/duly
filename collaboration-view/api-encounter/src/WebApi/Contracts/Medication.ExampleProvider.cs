// -----------------------------------------------------------------------
// <copyright file="Medication.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Swashbuckle.AspNetCore.Filters;
using System;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class MedicationExampleProvider : IExamplesProvider<Medication>
    {
        public Medication GetExamples()
        {
            return BuildExampleFor(MedicationScheduleType.Regular);
        }

        public Medication BuildExampleFor(MedicationScheduleType scheduleType)
        {
            switch (scheduleType)
            {
                case MedicationScheduleType.Regular:
                    return new Medication
                    {
                        Id = "Regular_Medication_Id",
                        ScheduleType = MedicationScheduleType.Regular,
                        Title = "Potassium chloride ER Capsule 10 mEq",
                        Reason = "For potassium replacement (COPD)",
                        StartDate = new DateTime(2021, 10, 15),
                        Provider = new PractitionerGeneralInfoExampleProvider().GetExamples(),
                        Instructions = "Take 1 capsule by month 2 times a day"
                    };

                case MedicationScheduleType.Other:
                default:
                    return new Medication
                    {
                        Id = "Other_Medication_Id",
                        ScheduleType = MedicationScheduleType.Other,
                        Title = "ProAir HFA Inhaler 90 mcg/inh",
                        Reason = "Breathing medications for (Asthma) attacks",
                        StartDate = new DateTime(2021, 9, 15),
                        Provider = new PractitionerGeneralInfoExampleProvider().GetExamples(),
                        Instructions = "For Adults and children 12 years and over: Use this medicine every 4 to 6 hours, or as needed. Inhale one to 2 puffs by month each time."
                    };
            }
        }
    }
}