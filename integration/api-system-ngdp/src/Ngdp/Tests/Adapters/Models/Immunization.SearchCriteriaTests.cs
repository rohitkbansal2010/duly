// <copyright file="Immunization.SearchCriteriaTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Ngdp.Adapter.Tests.Adapters.Models
{
    [TestFixture]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Tests")]
    public class ImmunizationSearchCriteriaTests
    {
        [Test]
        public void ConvertToParameters_Empty_Test()
        {
            //Arrange
            var searchCriteria = new ImmunizationSearchCriteria();

            //Act
            var result = searchCriteria.ConvertToParameters();

            //Assert
            string output = JsonConvert.SerializeObject(result);
            output.Should().Be("{\"PatientId\":null,\"IncludedDueStatusIds\":\"\"}");
        }

        [Test]
        public void ConvertToParameters_WithIncludedVisitTypeIds_Test()
        {
            //Arrange
            var searchCriteria = new ImmunizationSearchCriteria
            {
                IncludedDueStatusesIds = new[] { 1, 4, 1255 }
            };

            //Act
            var result = searchCriteria.ConvertToParameters();

            //Assert
            string output = JsonConvert.SerializeObject(result);
            output.Should().Be("{\"PatientId\":null,\"IncludedDueStatusIds\":\"1,4,1255\"}");
        }

        [Test]
        public void ConvertToParameters_WithPatientId_Test()
        {
            //Arrange
            var searchCriteria = new ImmunizationSearchCriteria
            {
                PatientId = "1234"
            };

            //Act
            var result = searchCriteria.ConvertToParameters();

            //Assert
            string output = JsonConvert.SerializeObject(result);
            output.Should().Be("{\"PatientId\":\"1234\",\"IncludedDueStatusIds\":\"\"}");
        }
    }
}