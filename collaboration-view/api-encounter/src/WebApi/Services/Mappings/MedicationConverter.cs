// <copyright file="MedicationConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using System;
using System.Linq;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    internal class MedicationConverter : ITypeConverter<Models.Medication, Medication>
    {
        public Medication Convert(
            Models.Medication source,
            Medication destination,
            ResolutionContext context)
        {
            var medication = new Medication
            {
                Id = source.Id,
                ScheduleType = GetMedicineScheduleType(source.Dosages),
                Title = GetMedicineTitle(source.Drug),
                Reason = GetMedicineReason(source.Reason),
                Instructions = GetMedicineInstructions(source.Dosages),
                StartDate = GetMedicineStartDate(source.Period),
                Provider = GetMedicineProvider(source.Prescriber, context)
            };

            return medication;
        }

        private static MedicationScheduleType GetMedicineScheduleType(Models.MedicationDosage[] dosages)
        {
            return dosages.Length == 1 && !dosages[0].AsNeeded
                ? MedicationScheduleType.Regular
                : MedicationScheduleType.Other;
        }

        private static string GetMedicineTitle(Models.Drug drug)
        {
            return drug?.Title;
        }

        private static string GetMedicineReason(Models.MedicationReason reason)
        {
            return reason == null ? null : string.Join(Environment.NewLine, reason.ReasonText);
        }

        private static string GetMedicineInstructions(Models.MedicationDosage[] dosages)
        {
            if (dosages == null || dosages.Length == 0)
            {
                return null;
            }

            var instructions = dosages
                .Where(x => !string.IsNullOrWhiteSpace(x.PatientInstruction))
                .Select(x => x.PatientInstruction)
                .ToArray();

            return instructions.Any()
                ? string.Join(Environment.NewLine, instructions)
                : null;
        }

        private static DateTime? GetMedicineStartDate(Models.Period period)
        {
            return period?.Start?.Date;
        }

        private static PractitionerGeneralInfo GetMedicineProvider(Models.PractitionerGeneralInfo prescriber, ResolutionContext context)
        {
            var practitionerGeneralInfo = context.Mapper.Map<PractitionerGeneralInfo>(prescriber);
            return practitionerGeneralInfo;
        }
    }
}