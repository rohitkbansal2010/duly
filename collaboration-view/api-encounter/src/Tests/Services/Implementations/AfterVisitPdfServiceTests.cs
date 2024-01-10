// <copyright file="AfterVisitPdfServiceTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
using Duly.CollaborationView.Encounter.Api.Services.Implementations;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using ApiContracts = Duly.CollaborationView.Encounter.Api.Contracts;
using Models =Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations
{
    [TestFixture]

    public class AfterVisitPdfServiceTests
    {
        private Mock<IAfterVisitPdfRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IAfterVisitPdfRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        public async Task PostImagingDetailsAsyncTest()
        {
            //Arrange
            //const string patientId = "123";
            //var patient = BuildPatient(patientId);
            var afterVisitPdf = new ApiContracts.AfterVisitPdf
            {
                PatientId = "test-patient-id",
                ProviderId = "test-provider-id",
                AfterVisitPDF = "test-after-visit-pdf",
                AppointmentId = "test-appointment-id"
            };

            var afterVisitPdf1 = new Models.AfterVisitPdf
            {
                PatientId = "test-patient-id",
                ProviderId = "test-provider-id",
                AfterVisitPDF = "test-after-visit-pdf",
                AppointmentId = "test-appointment-id"
            };

            int res = 2;

            _repositoryMock
                .Setup(repo => repo.PostAfterVisitPdfAsync(afterVisitPdf1))
                .Returns(Task.FromResult(res));

            _mapperMock
                .Setup(mapper => mapper.Map<Models.AfterVisitPdf>(afterVisitPdf))
                .Returns(afterVisitPdf1);

            var serviceMock = new AfterVisitPdfService(
                _mapperMock.Object,
                _repositoryMock.Object);

            //Act
            var results = await serviceMock.PostAfterVisitPdfAsync(afterVisitPdf);

            //Assert
            _repositoryMock.Verify(x => x.PostAfterVisitPdfAsync(afterVisitPdf1), Times.Once());

            results.Should().NotBe(0);
        }
    }
}
