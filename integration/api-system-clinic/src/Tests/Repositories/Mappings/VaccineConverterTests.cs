// <copyright file="VaccineConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Api.Tests.Common;
using Duly.Clinic.Contracts;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;

using FhirModel = Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class VaccineConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        private const string CvxType = "http://hl7.org/fhir/sid/cvx";

        [Test]
        public void ConvertTest()
        {
            //Arrange
            var source = new FhirModel.CodeableConcept
            {
                Text = "COVID-19",
                Coding = new()
                {
                    new()
                    {
                        System = CvxType,
                        Code = "CVD19"
                    }
                }
            };

            //Act
            var result = Mapper.Map<Vaccine>(source);

            //Assert
            result.Should().NotBeNull();
            result.Text.Should().Be(source.Text);

            result.CvxCodes.Should().HaveCount(source.Coding.Count);
            result.CvxCodes.First().Should().Be(source.Coding.First().Code);
        }

        [Test]
        public void IgnoreNonCvxCodes_Test()
        {
            //Arrange
            var source = new FhirModel.CodeableConcept
            {
                Text = "COVID-19",
                Coding = new()
                {
                    new()
                    {
                        System = "non-cvx",
                        Code = "CVD19"
                    }
                }
            };

            //Act
            var result = Mapper.Map<Vaccine>(source);

            //Assert
            result.Should().NotBeNull();
            result.Text.Should().Be(source.Text);
            result.CvxCodes.Should().BeEmpty();
        }
    }
}