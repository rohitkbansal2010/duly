// <copyright file="TestReportStatusConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Mappings;
using Duly.CollaborationView.Encounter.Api.Tests.Common;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Mappings
{
    [TestFixture]
    internal class TestReportStatusConverterTests : MapperConfigurator<RepositoryModelsToProcessContractsProfile>
    {
        [TestCase(DiagnosticReportStatus.Amended, TestReportStatus.Amended)]
        [TestCase(DiagnosticReportStatus.Appended, TestReportStatus.Appended)]
        [TestCase(DiagnosticReportStatus.Cancelled, TestReportStatus.Cancelled)]
        [TestCase(DiagnosticReportStatus.Corrected, TestReportStatus.Corrected)]
        [TestCase(DiagnosticReportStatus.Final, TestReportStatus.Final)]
        [TestCase(DiagnosticReportStatus.Partial, TestReportStatus.Partial)]
        [TestCase(DiagnosticReportStatus.Preliminary, TestReportStatus.Preliminary)]
        [TestCase(DiagnosticReportStatus.Registered, TestReportStatus.Registered)]
        [TestCase(DiagnosticReportStatus.Unknown, TestReportStatus.Unknown)]
        public void ConvertTest(DiagnosticReportStatus sourceDiagnosticReportStatus, TestReportStatus targetTestReportStatus)
        {
            //Arrange

            //Act
            var result = Mapper.Map<TestReportStatus>(sourceDiagnosticReportStatus);

            //Assert
            result.Should().Be(targetTestReportStatus);
        }

        [TestCase(DiagnosticReportStatus.EnteredInError)]
        public void ConvertTest_Throw_ServiceNotMappedException(DiagnosticReportStatus diagnosticReportStatus)
        {
            //Arrange
            var source = diagnosticReportStatus;

            //Act
            Action action = () => Mapper.Map<TestReportStatus>(source);

            //Assert
            var result = action.Should().ThrowExactly<ServiceNotMappedException>();
            result.Which.Message.Should().Be("Could not map DiagnosticReportStatus to TestReportStatus");
        }
    }
}
