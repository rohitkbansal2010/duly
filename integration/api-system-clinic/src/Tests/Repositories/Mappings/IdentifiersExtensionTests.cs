// <copyright file="IdentifiersExtensionTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Repositories.Mappings;
using FluentAssertions;
using Hl7.Fhir.Model;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class IdentifiersExtensionTests
    {
        [Test]
        public void ConvertIdentifiers_Test()
        {
            var ids = new List<Identifier>
            {
                new(default, "1405")
                {
                    Type = new CodeableConcept { Text = "External" }
                }
            };

            //Act
            var result = ids.ConvertIdentifiers();

            //Assert
            result.Should().NotBeNullOrEmpty();
            result.First().Should().Be("External|1405");
        }

        [Test]
        public void ConvertIdentifiers_Test_EmptyResult()
        {
            var ids = new List<Identifier>
            {
                new(default, "1405")
            };

            //Act
            var result = ids.ConvertIdentifiers();

            //Assert
            result.Should().BeEmpty();
        }
    }
}
