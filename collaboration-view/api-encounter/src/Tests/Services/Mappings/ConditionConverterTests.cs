// <copyright file="ConditionConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Services.Mappings;
using Duly.CollaborationView.Encounter.Api.Tests.Common;
using FluentAssertions;
using NUnit.Framework;
using System;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Mappings
{
    [TestFixture]
    internal class ConditionConverterTests : MapperConfigurator<RepositoryModelsToProcessContractsProfile>
    {
        [TestCase(Models.ConditionClinicalStatus.Active)]
        [TestCase(Models.ConditionClinicalStatus.Inactive)]
        public void ConvertTest(Models.ConditionClinicalStatus status)
        {
            //Arrange
            var name = "test-name";
            var date = new DateTimeOffset(2020, 01, 19, 0, 0, 0, default);

            var source = new Models.Condition
            {
                Id = Guid.NewGuid().ToString(),
                ClinicalStatus = status,
                Name = name,
                RecordedDate = date,
            };

            //Act
            var result = Mapper.Map<HealthCondition>(source);

            //Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(name);
            result.Date.Should().Be(date);
        }

        [Test]
        public void ConvertTest_ResolvedStatus()
        {
            //Arrange
            var name = "test-name";
            var date = new DateTimeOffset(2020, 01, 19, 0, 0, 0, default);

            var source = new Models.Condition
            {
                Id = Guid.NewGuid().ToString(),
                ClinicalStatus = Models.ConditionClinicalStatus.Resolved,
                Name = name,
                AbatementPeriod = new()
                {
                    End = date,
                }
            };

            //Act
            var result = Mapper.Map<HealthCondition>(source);

            //Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(name);
            result.Date.Should().Be(date);
        }

        [Test]
        public void ConvertTest_ResolvedStatus_NullEndDate()
        {
            //Arrange
            var name = "test-name";

            var source = new Models.Condition
            {
                Id = Guid.NewGuid().ToString(),
                ClinicalStatus = Models.ConditionClinicalStatus.Resolved,
                Name = name,
                AbatementPeriod = new()
            };

            //Act
            var result = Mapper.Map<HealthCondition>(source);

            //Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(name);
            result.Date.Should().BeNull();
        }

        [Test]
        public void ConvertTest_ResolvedStatus_NullAbatementPeriod()
        {
            //Arrange
            var name = "test-name";

            var source = new Models.Condition
            {
                Id = Guid.NewGuid().ToString(),
                ClinicalStatus = Models.ConditionClinicalStatus.Resolved,
                Name = name,
            };

            //Act
            var result = Mapper.Map<HealthCondition>(source);

            //Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(name);
            result.Date.Should().BeNull();
        }
    }
}
