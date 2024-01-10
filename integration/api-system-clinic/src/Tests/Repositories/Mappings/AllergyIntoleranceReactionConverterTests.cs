// <copyright file="AllergyIntoleranceReactionConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Api.Tests.Common;
using Duly.Clinic.Contracts;
using FluentAssertions;
using NUnit.Framework;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class AllergyIntoleranceReactionConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        [Test]
        public void Convert_Description_IsNull_Test()
        {
            //Arrange
            var source = new R4.AllergyIntolerance.ReactionComponent
            {
                Severity = R4.AllergyIntolerance.AllergyIntoleranceSeverity.Mild
            };

            //Act
            var result = Mapper.Map<AllergyIntoleranceReaction>(source);

            //Assert
            result.Title.Should().BeNull();
            result.Severity.Should().Be(AllergyIntoleranceReactionSeverity.Mild);
        }

        [Test]
        public void ConvertTest()
        {
            //Arrange
            var source = new R4.AllergyIntolerance.ReactionComponent
            {
                Description = "The name of the reaction.",
                Severity = R4.AllergyIntolerance.AllergyIntoleranceSeverity.Severe
            };

            //Act
            var result = Mapper.Map<AllergyIntoleranceReaction>(source);

            //Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(source.Description);
            result.Severity.Should().Be(AllergyIntoleranceReactionSeverity.Severe);
        }
   }
}