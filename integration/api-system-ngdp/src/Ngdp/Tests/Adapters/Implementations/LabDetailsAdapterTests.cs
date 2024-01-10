// <copyright file="LabDetailsAdapterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Adapter;
using Duly.Ngdp.Adapter.Adapters.Implementations;
using Duly.Ngdp.Adapter.Adapters.Models;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ngdp.Adapter.Tests.Adapters.Implementations
{
    [TestFixture]
    public class LabDetailsAdapterTests
    {
        private const string PostLabDetailsProcedureName = "[InsertLabDetails]";

        private Mock<ICVDapperContext> _mockedDapperContext;

        [SetUp]
        public void Setup()
        {
            _mockedDapperContext = new Mock<ICVDapperContext>();
        }

        [Test]
        public async Task PostLabDetailsAsync_Test()
        {
            //Arrange
            //const int referralId = 123456677;

            var labDetails = new LabDetails
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
            var adapter = new LabDetailsAdapter(_mockedDapperContext.Object);
            var labDetails1 = SetupDapperContext(labDetails);

            //Act
            var result = await adapter.PostLabDetailsAsync(labDetails);

            //Assert
            Assert.AreEqual(result, labDetails1);
        }

        private int SetupDapperContext(LabDetails labDetails)
        {
            int res = 2;

            _mockedDapperContext
                .Setup(context =>
                    context.ExecuteScalarAsync<int>(PostLabDetailsProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(res);

            return res;
        }
    }
}