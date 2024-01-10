// <copyright file="NgdpLabDetailsRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Models;
using Duly.Ngdp.Api.Repositories.Implementations;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class NgdpLabDetailsRepositoryTests
    {
        private Mock<IMapper> _mapperMocked;
        private Mock<ILabDetailsAdapter> _adapterMocked;

        [SetUp]
        public void SetUp()
        {
            _adapterMocked = new Mock<ILabDetailsAdapter>();
            _mapperMocked = new Mock<IMapper>();
        }

        [Test]
        public async Task PostLabDetailsAsync_Test()
        {
            //Arrange
            //const string referralId = "1";
            var labDetails = new Contracts.LabDetails
            {
                Type = "L",
                Lab_ID = "test-lab-id",
                Lab_Location = "test-lab-location",
                Lab_Name = "test-lab-name",
                CreatedDate = new System.DateTime(2008, 5, 1, 8, 6, 32),
                Appointment_ID = "test-appointment-id",
                Patient_ID = "test-patient-id",
                Skipped = false
            };

            var labDetailsConverted = SetupMapper(labDetails);
            var labDetails1 = SetupAdapter(labDetailsConverted);

            var repository = new NgdpLabDetailsRepository(_adapterMocked.Object, _mapperMocked.Object);

            //Act
            var result = await repository
                .PostLabDetailsAsync(labDetails);

            //Assert
            Assert.AreEqual(result, labDetails1);
        }

        private int SetupAdapter(LabDetails labDetails)
        {
            int res = 2;

            _adapterMocked
                .Setup(adapter => adapter.PostLabDetailsAsync(labDetails))
                .ReturnsAsync(res);

            return res;
        }

        private AdapterModels.LabDetails SetupMapper(Contracts.LabDetails labDetails)
        {
            AdapterModels.LabDetails labdetails1 = new();
            labdetails1.Type = labDetails.Type;
            labdetails1.Lab_ID = labDetails.Lab_ID;
            labdetails1.Lab_Location = labDetails.Lab_Location;
            labdetails1.Lab_Name = labDetails.Lab_Name;
            labdetails1.CreatedDate = labDetails.CreatedDate;
            labdetails1.Appointment_ID = labDetails.Appointment_ID;
            labdetails1.Patient_ID = labDetails.Patient_ID;
            labdetails1.Skipped = labDetails.Skipped;

            _mapperMocked
                .Setup(mapper => mapper.Map<AdapterModels.LabDetails>(labDetails))
                .Returns(labdetails1);

            return labdetails1;
        }
    }
}
