// <copyright file="AttachmentConverterTests.cs" company="Duly Health and Care">
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
    public class AttachmentConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        [Test]
        public void ConvertTest()
        {
            //Arrange
            R4.Attachment source = new()
            {
                Title = "testTitle",
                ContentType = "test/ContentType",
                Data = new byte[] { 1, 2, 5, 16, 12 },
                Size = 123,
                Url = "testUrl"
            };

            //Act
            var result = Mapper.Map<Attachment>(source);

            //Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("testTitle");
            result.ContentType.Should().Be("test/ContentType");
            result.Size.Should().Be(123);
            result.Url.Should().Be("testUrl");
            result.Data.Should().NotBeNullOrEmpty();
        }
    }
}