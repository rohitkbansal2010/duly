// <copyright file="Appointment.SearchCriteriaTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using System;

namespace Ngdp.Adapter.Tests.Adapters.Models
{
    [TestFixture]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Tests")]
    public class AppointmentSearchCriteriaTests
    {
        [Test]
        public void ConvertToParameters_Empty_Test()
        {
            //Arrange
            var appointmentSearchCriteria = new AppointmentSearchCriteria();

            //Act
            var result = appointmentSearchCriteria.ConvertToParameters();

            //Assert
            string output = JsonConvert.SerializeObject(result);
            output.Should()
                .Be(
                    "{\"DepartmentId\":null,\"ApptTimeLowerBound\":\"0001-01-01T00:00:00\",\"ApptTimeUpperBound\":\"0001-01-01T00:00:00\",\"IncludedVisitTypeIds\":\"\",\"ExcludedStatuses\":\"\"}");
        }

        [Test]
        public void ConvertToParameters_WithIncludedVisitTypeIds_Test()
        {
            //Arrange
            var appointmentSearchCriteria = new AppointmentSearchCriteria
            {
                IncludedVisitTypeIds = new[] { "xxx", "xxxx", "xxxxx" }
            };

            //Act
            var result = appointmentSearchCriteria.ConvertToParameters();

            //Assert
            string output = JsonConvert.SerializeObject(result);
            output.Should()
                .Be(
                    "{\"DepartmentId\":null,\"ApptTimeLowerBound\":\"0001-01-01T00:00:00\",\"ApptTimeUpperBound\":\"0001-01-01T00:00:00\"," +
                    "\"IncludedVisitTypeIds\":\"" +
                    "xxx,xxxx,xxxxx" +
                    "\",\"ExcludedStatuses\":\"\"}");
        }

        [TestCase(AppointmentStatus.Canceled)]
        [TestCase(AppointmentStatus.Arrived)]
        [TestCase(AppointmentStatus.Completed)]
        [TestCase(AppointmentStatus.Scheduled)]
        [TestCase(AppointmentStatus.Unresolved)]
        public void ConvertToParameters_AppointmentStatusToString_Test(AppointmentStatus status)
        {
            //Arrange
            var appointmentSearchCriteria = new AppointmentSearchCriteria
            {
                ExcludedStatuses = new[]
                {
                    status
                }
            };

            //Act
            var result = appointmentSearchCriteria.ConvertToParameters();

            //Assert
            string output = JsonConvert.SerializeObject(result);
            output.Should()
                .Be(
                    "{\"DepartmentId\":null,\"ApptTimeLowerBound\":\"0001-01-01T00:00:00\",\"ApptTimeUpperBound\":\"0001-01-01T00:00:00\",\"IncludedVisitTypeIds\":\"\"," +
                    "\"ExcludedStatuses\":\"" +
                    status +
                    "\"}");
        }

        [TestCase(AppointmentStatus.LeftWithoutSeen, "Left without seen")]
        [TestCase(AppointmentStatus.NoShow, "No Show")]
        [TestCase(AppointmentStatus.ChargeEntered, "Charge Entered")]
        public void ConvertToParameters_AppointmentStatusToString_Complex_Test(AppointmentStatus status, string targetText)
        {
            //Arrange
            var appointmentSearchCriteria = new AppointmentSearchCriteria
            {
                ExcludedStatuses = new[]
                {
                    status
                }
            };

            //Act
            var result = appointmentSearchCriteria.ConvertToParameters();

            //Assert
            string output = JsonConvert.SerializeObject(result);
            output.Should()
                .Be(
                    "{\"DepartmentId\":null,\"ApptTimeLowerBound\":\"0001-01-01T00:00:00\",\"ApptTimeUpperBound\":\"0001-01-01T00:00:00\",\"IncludedVisitTypeIds\":\"\"," +
                    "\"ExcludedStatuses\":\"" +
                    targetText +
                    "\"}");
        }

        [Test]
        public void ConvertToParameters_AppointmentStatusToString_ArgumentOutOfRangeException_Test()
        {
            //Arrange
            var appointmentSearchCriteria = new AppointmentSearchCriteria
            {
                ExcludedStatuses = new[]
                {
                    (AppointmentStatus)(-1)
                }
            };

            //Act
            Action action = () => appointmentSearchCriteria.ConvertToParameters();

            //Assert
            var result = action.Should().Throw<ArgumentOutOfRangeException>();
            result.Which.Message.Should().Be("Specified argument was out of the range of valid values. (Parameter 'appointmentStatus')");
        }
    }
}
