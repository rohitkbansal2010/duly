// <copyright file="CodingsComparerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Extensions;
using FluentAssertions;
using Hl7.Fhir.Model;
using NUnit.Framework;

namespace Fhir.Adapter.Tests.Extensions
{
    [TestFixture]
    public class CodingsComparerTests
    {
        protected const string SystemLoinc = "http://loinc.org";

        [Test]
        public void EqualsTest_True()
        {
            //Arrange
            var comparer = new CodingsComparer();

            //Act
            var result = comparer.Equals(new Coding(default, "101"), new Coding(default, "101"));

            //Assert
            result.Should().BeTrue();
        }

        [Test]
        public void EqualsTest_False()
        {
            //Arrange
            var comparer = new CodingsComparer();

            //Act
            var result = comparer.Equals(new Coding(default, "101"), new Coding(default, "102"));

            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public void EqualsTest_NullX()
        {
            //Arrange
            var comparer = new CodingsComparer();

            //Act
            var result = comparer.Equals(null, new Coding());

            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public void GetHashCodeTest()
        {
            //Arrange
            var comparer = new CodingsComparer();

            //Act
            var result1 = comparer.GetHashCode(new Coding(SystemLoinc, "101"));
            var result2 = comparer.GetHashCode(new Coding(SystemLoinc, "101"));

            //Assert
            result1.Should().Be(result2);
        }
    }
}
