// <copyright file="AzureActiveDirectoryUserAccountIdentityConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Services.Mappings;
using Duly.CollaborationView.Encounter.Api.Tests.Common;
using Duly.Common.Security.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Mappings
{
    [TestFixture]
    internal class AzureActiveDirectoryUserAccountIdentityConverterTests :
        MapperConfigurator<RepositoryModelsToProcessContractsProfile>
    {
        [Test]
        public void Convert_ToPractitionerGeneralInfo_Test()
        {
            //Arrange
            var source = new AzureActiveDirectoryUserAccountIdentity
            {
                Id = Guid.NewGuid().ToString(),
                LastName = "test",
                FirstName = "fTest",
                Photo = "testPhoto",
                ContentType = "testContentType"
            };

            //Act
            var result = Mapper.Map<PractitionerGeneralInfo>(source);

            //Assert
            result.Id.Should().Be(source.Id);
            result.HumanName.FamilyName.Should().Be(source.LastName);
            result.HumanName.GivenNames.Single().Should().Be(source.FirstName);
            result.Photo.Data.Should().Be(source.Photo);
            result.Photo.ContentType.Should().Be(source.ContentType);
        }

        [Test]
        public void Convert_ToParty_Test()
        {
            //Arrange
            var source = new AzureActiveDirectoryUserAccountIdentity
            {
                Id = Guid.NewGuid().ToString(),
                LastName = "test",
                FirstName = "fTest",
                Photo = "testPhoto",
                ContentType = "testContentType"
            };

            //Act
            var result = Mapper.Map<Party>(source);

            //Assert
            result.Id.Should().Be(source.Id);
            result.HumanName.FamilyName.Should().Be(source.LastName);
            result.HumanName.GivenNames.Single().Should().Be(source.FirstName);
            result.Photo.Data.Should().Be(source.Photo);
            result.Photo.ContentType.Should().Be(source.ContentType);
            result.MemberType.Should().Be(MemberType.Unknown);
        }

        [Test]
        public void Convert_ToParty_NamesAreNull_Test()
        {
            //Arrange
            var source = new AzureActiveDirectoryUserAccountIdentity
            {
                LastName = null,
                FirstName = null
            };

            //Act
            var result = Mapper.Map<Party>(source);

            //Assert
            result.HumanName.FamilyName.Should().BeNull();
            result.HumanName.GivenNames.Should().BeEmpty();
            result.HumanName.Prefixes.Should().BeEmpty();
            result.HumanName.Suffixes.Should().BeEmpty();
        }

        [Test]
        public void Convert_ToUser_Test()
        {
            //Arrange
            var source = new AzureActiveDirectoryUserAccountIdentity
            {
                Id = Guid.NewGuid().ToString(),
                LastName = "test",
                FirstName = "fTest",
                Photo = "testPhoto",
                ContentType = "testContentType"
            };

            //Act
            var result = Mapper.Map<User>(source);

            //Assert
            result.Id.Should().Be(source.Id);
            result.Name.FamilyName.Should().Be(source.LastName);
            result.Name.GivenNames.Single().Should().Be(source.FirstName);
            result.Photo.Data.Should().Be(source.Photo);
            result.Photo.ContentType.Should().Be(source.ContentType);
        }
    }
}
