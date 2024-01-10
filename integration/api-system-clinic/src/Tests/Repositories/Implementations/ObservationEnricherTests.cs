// <copyright file="ObservationEnricherTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Repositories.Implementations;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace Duly.Clinic.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class ObservationEnricherTests
    {
        private Mock<IEnrichBMI> _bmiEnricherMocked;

        [SetUp]
        public void SetUp()
        {
            _bmiEnricherMocked = new Mock<IEnrichBMI>();
        }

        [Test]
        public void EnrichResultsTest()
        {
            var observationTypes = new[] { ObservationType.BodyHeight, ObservationType.BodyWeight, ObservationType.BodyMassIndex };
            IEnumerable<Observation> observationsToEnrcih = new Observation[] { new Observation() { Type = ObservationType.BodyMassIndex } };
            var sourceObservations = new Observation[]
            {
                new Observation() { Type = ObservationType.BodyWeight },
                new Observation() { Type = ObservationType.BodyHeight }
            };

            var observationEnricher = new ObservationEnricher(_bmiEnricherMocked.Object);

            observationEnricher
                .Invoking(x => x.EnrichResults(observationsToEnrcih, sourceObservations)).Should()
                .NotThrow();

            _bmiEnricherMocked.Verify(x => x.EnrichBMI(It.IsAny<Observation>(), It.IsAny<IEnumerable<Observation>>()));
        }
    }
}