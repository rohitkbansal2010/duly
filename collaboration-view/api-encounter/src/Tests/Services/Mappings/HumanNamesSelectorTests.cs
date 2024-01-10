// <copyright file="HumanNamesSelectorTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Mappings;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Mappings
{
    [TestFixture]
    public class HumanNamesSelectorTests
    {
        [Test]
        public void SelectHumanNameByUse_ThrowException_Test()
        {
            //Arrange
            var names = Array.Empty<HumanName>();

            //Act
            Action action = () => HumanNamesSelector.SelectHumanNameByUse(names);

            //Assert
            var result = action.Should().Throw<ServiceNotMappedException>();
            result.Which.Message.Should().Be("Could not find name with proper use");
        }

        [Test]
        public void SelectHumanNameByUse_Usual_Test()
        {
            //Arrange
            var names = new HumanName[]
            {
                new()
                {
                    Use = NameUse.Nickname
                },
                new()
                {
                    Use = NameUse.Usual
                }
            };

            //Act
            var result = HumanNamesSelector.SelectHumanNameByUse(names);

            //Assert
            result.Should().Be(names[1]);
        }

        [Test]
        public void SelectHumanNameByUse_OfficialAndUsual_Test()
        {
            //Arrange
            var names = new HumanName[]
            {
                new()
                {
                    Use = NameUse.Official
                },
                new()
                {
                    Use = NameUse.Usual
                }
            };

            //Act
            var result = HumanNamesSelector.SelectHumanNameByUse(names);

            //Assert
            result.Should().Be(names[1]);
        }

        [Test]
        public void SelectHumanNameByUse_Official_Test()
        {
            //Arrange
            var names = new HumanName[]
            {
                new()
                {
                    Use = NameUse.Official
                },
                new()
                {
                    Use = NameUse.Anonymous
                }
            };

            //Act
            var result = HumanNamesSelector.SelectHumanNameByUse(names);

            //Assert
            result.Should().Be(names[0]);
        }
    }
}
