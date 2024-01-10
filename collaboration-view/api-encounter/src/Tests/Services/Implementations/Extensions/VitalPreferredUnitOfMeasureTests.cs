// <copyright file="VitalPreferredUnitOfMeasureTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Implementations.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations.Extensions
{
    [TestFixture]
    public class VitalPreferredUnitOfMeasureTests
    {
        [Test]
        public void SelectMeasurement_preferredUnitOfMeasure_isNullOrEmpty_Test()
        {
            //Act
            var observationComponent = new ObservationComponent
            {
                Measurements = new ObservationMeasurement[]
                {
                    new(),
                    new()
                }
            };

            string preferredUnitOfMeasure = null;

            //Arrange
            var result = VitalPreferredUnitOfMeasure.SelectMeasurement(observationComponent, preferredUnitOfMeasure);

            //Assert
            result.Should().Be(observationComponent.Measurements[0]);
        }

        [Test]
        public void SelectMeasurement_measurementsLength_lessThan2_Test()
        {
            //Act
            var observationComponent = new ObservationComponent
            {
                Measurements = new ObservationMeasurement[]
                {
                    new()
                }
            };

            const string preferredUnitOfMeasure = "test";

            //Arrange
            var result = VitalPreferredUnitOfMeasure.SelectMeasurement(observationComponent, preferredUnitOfMeasure);

            //Assert
            result.Should().Be(observationComponent.Measurements[0]);
        }

        [Test]
        public void SelectMeasurement_FindIndex_NotFound_Test()
        {
            //Act
            var observationComponent = new ObservationComponent
            {
                Measurements = new ObservationMeasurement[]
                {
                    new(),
                    new()
                }
            };

            const string preferredUnitOfMeasure = "test";

            //Arrange
            var result = VitalPreferredUnitOfMeasure.SelectMeasurement(observationComponent, preferredUnitOfMeasure);

            //Assert
            result.Should().Be(observationComponent.Measurements[0]);
        }

        [Test]
        public void SelectMeasurement_FindIndex_FoundIndex_Test()
        {
            //Act
            const string preferredUnitOfMeasure = "test";

            var observationComponent = new ObservationComponent
            {
                Measurements = new ObservationMeasurement[]
                {
                    new(),
                    new()
                    {
                        Unit = preferredUnitOfMeasure
                    }
                }
            };

            //Arrange
            var result = VitalPreferredUnitOfMeasure.SelectMeasurement(observationComponent, preferredUnitOfMeasure);

            //Assert
            result.Should().Be(observationComponent.Measurements[1]);
        }

        [Test]
        public void SelectComponent_IndexNotFound_Test()
        {
            //Act
            var observation = new Observation
            {
                Components = new[]
                {
                    new ObservationComponent(),
                    new ObservationComponent()
                }
            };

            ObservationComponentType? componentType = ObservationComponentType.Diastolic;

            //Arrange
            var result = VitalPreferredUnitOfMeasure.SelectComponent(observation, componentType);

            //Assert
            result.Should().Be(observation.Components[0]);
        }

        [Test]
        public void SelectComponent_IndexFound_Test()
        {
            //Act
            var observation = new Observation
            {
                Components = new[]
                {
                    new ObservationComponent(),
                    new ObservationComponent
                    {
                        Type = ObservationComponentType.Systolic
                    }
                }
            };

            ObservationComponentType? componentType = ObservationComponentType.Systolic;

            //Arrange
            var result = VitalPreferredUnitOfMeasure.SelectComponent(observation, componentType);

            //Assert
            result.Should().Be(observation.Components[1]);
        }

        [Test]
        public void ChooseLabel_Height_Test()
        {
            //Act
            var component = new ObservationComponent();
            var observationType = ObservationType.BodyHeight;

            //Arrange
            var result = VitalPreferredUnitOfMeasure.ChooseLabel(component, observationType);

            //Assert
            result.Should().Be("Height");
        }

        [Test]
        public void ChooseLabel_observationComponentType_Test()
        {
            //Act
            var component = new ObservationComponent
            {
                Type = ObservationComponentType.Diastolic
            };
            var observationType = ObservationType.BodyHeight;

            //Arrange
            var result = VitalPreferredUnitOfMeasure.ChooseLabel(component, observationType);

            //Assert
            result.Should().Be("Diastolic");
        }
    }
}
