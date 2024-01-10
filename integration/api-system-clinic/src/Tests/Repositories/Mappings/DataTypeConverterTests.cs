// <copyright file="DataTypeConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Repositories.Mappings;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class DataTypeConverterTests
    {
        [Test]
        public void ConvertTest_BloodPressure()
        {
            var converter = new DataTypeConverter();

            converter.Invoking(x => x.Convert(null, default(DateTimeOffset), null)).Should().Throw<ConceptNotMappedException>();
        }
    }
}
