// <copyright file="ImmunizationsRecommendedGroupConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Mappings;
using Duly.CollaborationView.Encounter.Api.Tests.Common;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Mappings
{
    [TestFixture]
    internal class ImmunizationsRecommendedGroupConverterTests : MapperConfigurator<RepositoryModelsToProcessContractsProfile>
    {
        public const string Duetitle = "DUE";

        [Test]
        public void ConvertTest()
        {
            var vaccineName = "test-vac-name";
            var dateTime = DateTime.Now;
            var status = RecommendedImmunizationStatus.DueOn;
            var recommendedImmunization = new RecommendedImmunization()
            {
                VaccineName = vaccineName,
                DueDate = dateTime,
                Status = status,
            };
            var immunizationsRecommendedGroup = new ImmunizationsRecommendedGroup()
            {
                Title = recommendedImmunization.VaccineName,
                Vaccinations = new[]
                {
                    new RecommendedVaccination()
                    {
                        Date = recommendedImmunization.DueDate,
                        DateTitle = Duetitle,
                        Status = Mapper.Map<RecommendedVaccinationStatus>(status)
                    }
                }
            };

            var convertResult = Mapper.Map<ImmunizationsRecommendedGroup>(recommendedImmunization);

            convertResult.Title.Should().BeEquivalentTo(immunizationsRecommendedGroup.Title);
            convertResult.Vaccinations.First().DateTitle.Should().Be(immunizationsRecommendedGroup.Vaccinations.First().DateTitle);
            convertResult.Vaccinations.First().Status.Should().Be(immunizationsRecommendedGroup.Vaccinations.First().Status);
            convertResult.Vaccinations.First().Date.Should().Be(immunizationsRecommendedGroup.Vaccinations.First().Date);
        }
    }
}
