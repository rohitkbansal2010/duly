// <copyright file="PatientAppointmentStatusConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Services.Mappings;
using Duly.CollaborationView.Encounter.Api.Tests.Common;
using FluentAssertions;
using NUnit.Framework;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Mappings
{
    [TestFixture]
    internal class PatientAppointmentStatusConverterTests : MapperConfigurator<RepositoryModelsToProcessContractsProfile>
    {
        [TestCase(Models.AppointmentStatus.Arrived, PatientAppointmentStatus.Unknown)]
        [TestCase(Models.AppointmentStatus.Canceled, PatientAppointmentStatus.Cancelled)]
        [TestCase(Models.AppointmentStatus.Completed, PatientAppointmentStatus.Completed)]
        [TestCase(Models.AppointmentStatus.LeftWithoutSeen, PatientAppointmentStatus.Unknown)]
        [TestCase(Models.AppointmentStatus.NoShow, PatientAppointmentStatus.NoShow)]
        [TestCase(Models.AppointmentStatus.Scheduled, PatientAppointmentStatus.Scheduled)]
        [TestCase(Models.AppointmentStatus.Unresolved, PatientAppointmentStatus.Unknown)]
        [TestCase(Models.AppointmentStatus.ChargeEntered, PatientAppointmentStatus.Unknown)]
        [TestCase(Models.AppointmentStatus.Unknown, PatientAppointmentStatus.Unknown)]
        public void ConvertTest(Models.AppointmentStatus sourceAppointmentStatus, PatientAppointmentStatus targetPatientAppointmentStatus)
        {
            //Arrange

            //Act
            var result = Mapper.Map<PatientAppointmentStatus>(sourceAppointmentStatus);

            //Assert
            result.Should().Be(targetPatientAppointmentStatus);
        }
    }
}
