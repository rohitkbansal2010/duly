// <copyright file="VaccinationConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using System.Text;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    internal class VaccinationConverter : ITypeConverter<Models.PastImmunization, Vaccination>
    {
        private const string Separator = @"\r\n";

        public Vaccination Convert(
            Models.PastImmunization source,
            Vaccination destination,
            ResolutionContext context)
        {
            var vaccination = new Vaccination
            {
                Title = source.Vaccine.Text,
                DateTitle = BuildDateTitle(source),
                Date = source.OccurrenceDateTime,
                Dose = BuildDose(source.Dose),
                Notes = BuildNotes(source)
            };

            return vaccination;
        }

        private static string BuildDateTitle(Models.PastImmunization source)
        {
            return source.Status == Models.PastImmunizationStatus.Completed ? Immunizations.AdministeredTitle : Immunizations.NotAdministeredTitle;
        }

        private static Dose BuildDose(Models.Quantity dose)
        {
            if (dose?.Value == null)
                return null;

            return new Dose
            {
                Amount = dose.Value.Value,
                Unit = dose.Unit
            };
        }

        private static string BuildNotes(Models.PastImmunization source)
        {
            var sb = new StringBuilder();

            if (source.Status == Models.PastImmunizationStatus.NotDone
                && !string.IsNullOrEmpty(source.StatusReason?.Reason))
                sb.Append($"{source.StatusReason.Reason}{Separator}");

            if (source.Notes?.Length > 0)
                sb.Append(string.Join(Separator, source.Notes));

            return sb.Length > 0 ? sb.ToString() : null;
        }
    }
}