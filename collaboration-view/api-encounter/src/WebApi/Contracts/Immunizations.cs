// <copyright file="Immunizations.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Represents an information about a patient's immunization, including information on recommended and past immunizations, as well as the patient's immunization progress.")]
    public class Immunizations
    {
        public const string AdministeredTitle = "ADMINISTERED";
        public const string NotAdministeredTitle = "NOT ADMINISTERED";

        [Description("Information on the progress of a patient's immunization.")]
        public ImmunizationsProgress Progress => CalculateProgress();

        [Description("Information on recommended immunizations.")]
        [Required]
        public ImmunizationsRecommendedGroup[] RecommendedImmunizations { get; set; }

        [Description("Information on past immunizations.")]
        [Required]
        public ImmunizationsGroup[] PastImmunizations { get; set; }

        private ImmunizationsProgress CalculateProgress()
        {
            var total = RecommendedImmunizations?
                .SelectMany(x => x.Vaccinations)
                .Count();
            total += PastImmunizations?
                .SelectMany(x => x.Vaccinations)
                .Count();

            if (total.GetValueOrDefault() == 0)
                return null;

            var completed = RecommendedImmunizations?
                .SelectMany(x => x.Vaccinations
                    .Where(v => v.Status is RecommendedVaccinationStatus.Completed or RecommendedVaccinationStatus.Addressed))
                .Count();
            completed += PastImmunizations?
                .SelectMany(x => x.Vaccinations
                    .Where(v => v.DateTitle == AdministeredTitle))
                .Count();

            var totalNumberOfRecommended = RecommendedImmunizations?.Length;

            return new ImmunizationsProgress
            {
                RecommendedGroupNumber = totalNumberOfRecommended.GetValueOrDefault(),
                PercentageCompletion = 100 * completed.GetValueOrDefault() / total.GetValueOrDefault()
            };
        }
    }
}