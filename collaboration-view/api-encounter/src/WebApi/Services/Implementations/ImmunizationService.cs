// <copyright file="ImmunizationService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Services.Extensions;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IImmunizationService"/>
    /// </summary>
    internal class ImmunizationService : IImmunizationService
    {
        private const string GroupNameForOtherPastImmunizations = "Other Past Immunizations";

        private static readonly Models.PastImmunizationStatus[] SupportedPastImmunizationStatues =
        {
            Models.PastImmunizationStatus.Completed,
            Models.PastImmunizationStatus.NotDone
        };

        private static readonly Models.RecommendedImmunizationStatus[] SupportedRecommendedImmunizationStatues =
        {
            Models.RecommendedImmunizationStatus.DueSoon,
            Models.RecommendedImmunizationStatus.Overdue
        };

        private readonly IPastImmunizationRepository _pastImmunizationRepository;
        private readonly IRecommendedImmunizationRepository _recommendedImmunizationRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;
        private readonly ICvxCodeRepository _cvxCodeRepository;

        public ImmunizationService(
            IMapper mapper,
            IPastImmunizationRepository pastImmunizationRepository,
            IRecommendedImmunizationRepository recommendedImmunizationRepository,
            IPatientRepository patientRepository,
            ICvxCodeRepository cvxCodeRepository)
        {
            _mapper = mapper;
            _pastImmunizationRepository = pastImmunizationRepository;
            _recommendedImmunizationRepository = recommendedImmunizationRepository;
            _patientRepository = patientRepository;
            _cvxCodeRepository = cvxCodeRepository;
        }

        public async Task<Immunizations> GetImmunizationsByPatientIdAsync(string patientId)
        {
            var immunizations = new Immunizations
            {
                RecommendedImmunizations = await BuildRecommendedImmunizationsAsync(patientId),
                PastImmunizations = await BuildPastImmunizationsAsync(patientId)
            };

            return immunizations;
        }

        private static string FindVaccineGroupName(
            IReadOnlyDictionary<string, string> cvxCodeVaccineGroupNameDictionary,
            IEnumerable<string> cvxCodes)
        {
            if (cvxCodeVaccineGroupNameDictionary == null)
                return GroupNameForOtherPastImmunizations;

            var foundCvxCode = cvxCodes?.FirstOrDefault(cvxCodeVaccineGroupNameDictionary.ContainsKey);
            if (foundCvxCode == null)
                return GroupNameForOtherPastImmunizations;

            return cvxCodeVaccineGroupNameDictionary[foundCvxCode];
        }

        private async Task<ImmunizationsRecommendedGroup[]> BuildRecommendedImmunizationsAsync(string patientId)
        {
            var patient = await _patientRepository.GetPatientByIdAsync(patientId);
            var ngdpPatientId = patient.Identifiers.FindIdWithExternalPrefix().SplitIdWithExternalPrefix();

            var recommendedImmunizations = await _recommendedImmunizationRepository.GetRecommendedImmunizationsForPatientAsync(
                ngdpPatientId,
                SupportedRecommendedImmunizationStatues);

            var immunizationsRecommendedGroups = _mapper.Map<ImmunizationsRecommendedGroup[]>(recommendedImmunizations);

            return immunizationsRecommendedGroups
                .OrderBy(group => group.Vaccinations.First().Date)
                .ThenBy(x => x.Title)
                .ToArray();
        }

        private async Task<ImmunizationsGroup[]> BuildPastImmunizationsAsync(string patientId)
        {
            var pastImmunizations = await _pastImmunizationRepository.GetPastImmunizationsForPatientAsync(
                    patientId,
                    SupportedPastImmunizationStatues);

            var immunizationsGroups = await GroupingPastImmunizationsAsync(pastImmunizations);

            return immunizationsGroups
                .OrderByDescending(x => x.Vaccinations.First().Date)
                .ThenBy(x => x.Title)
                .ToArray();
        }

        private async Task<IEnumerable<ImmunizationsGroup>> GroupingPastImmunizationsAsync(
            IEnumerable<Models.PastImmunization> pastImmunizations)
        {
            var immunizations = pastImmunizations.ToArray();
            var allCodes = immunizations
                .Where(x => x.Vaccine.CvxCodes != null)
                .SelectMany(x => x.Vaccine.CvxCodes)
                .Distinct()
                .ToArray();

            var cvxCodeVaccineGroupNameDictionary = await _cvxCodeRepository.FindVaccineGroupNamesByCodesAsync(allCodes);

            var orderedPastImmunizations = immunizations
                .OrderByDescending(x => x.OccurrenceDateTime)
                .ThenBy(x => x.Vaccine.Text);

            return BuildPastImmunizationGroups(orderedPastImmunizations, cvxCodeVaccineGroupNameDictionary);
        }

        private IEnumerable<ImmunizationsGroup> BuildPastImmunizationGroups(
            IEnumerable<Models.PastImmunization> orderedPastImmunizations,
            IReadOnlyDictionary<string, string> cvxCodeVaccineGroupNameDictionary)
        {
            var groups = orderedPastImmunizations.GroupBy(
                immunization => FindVaccineGroupName(cvxCodeVaccineGroupNameDictionary, immunization.Vaccine.CvxCodes));

            foreach (var group in groups)
            {
                yield return new ImmunizationsGroup
                {
                    Title = group.Key,
                    Vaccinations = group.Select(immunization => _mapper.Map<Vaccination>(immunization)).ToArray()
                };
            }
        }
    }
}
