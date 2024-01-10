// <copyright file="ConditionConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Api.Tests.Common;
using Duly.Clinic.Contracts;
using FluentAssertions;
using Hl7.Fhir.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using FhirModel = Hl7.Fhir.Model;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class ConditionConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        private const string ClinicalStatusCodeSystem = "http://terminology.hl7.org/CodeSystem/condition-clinical";
        private const string ClinicalStatusVersion = "4.0.0";

        [TestCase(R4.Condition.ConditionClinicalStatusCodes.Active)]
        [TestCase(R4.Condition.ConditionClinicalStatusCodes.Inactive)]
        [TestCase(R4.Condition.ConditionClinicalStatusCodes.Recurrence)]
        [TestCase(R4.Condition.ConditionClinicalStatusCodes.Relapse)]
        [TestCase(R4.Condition.ConditionClinicalStatusCodes.Remission)]
        [TestCase(R4.Condition.ConditionClinicalStatusCodes.Resolved)]
        public void ConvertTest(R4.Condition.ConditionClinicalStatusCodes clinicalStatusCode)
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            var recordedDate = new DateTimeOffset(DateTime.UtcNow);
            var start = recordedDate.AddDays(-120);
            var end = recordedDate.AddHours(24);

            var source = new R4.Condition
            {
                Id = id,
                Abatement = new FhirModel.Period
                {
                    StartElement = new FhirModel.FhirDateTime(start),
                    EndElement = new FhirModel.FhirDateTime(end)
                },
                ClinicalStatus = new FhirModel.CodeableConcept()
                {
                    Coding = new List<FhirModel.Coding>
                    {
                        new FhirModel.Coding
                        {
                            System = ClinicalStatusCodeSystem,
                            Version = ClinicalStatusVersion,
                            Code = clinicalStatusCode.GetLiteral()
                        }
                    }
                },
                Code = new FhirModel.CodeableConcept("system", "code", "Problem name"),
                RecordedDateElement = new FhirModel.FhirDateTime(recordedDate)
            };

            var expectedStatus = CalculateConditionClinicalStatus(clinicalStatusCode);

            switch (clinicalStatusCode)
            {
                case R4.Condition.ConditionClinicalStatusCodes.Active:
                case R4.Condition.ConditionClinicalStatusCodes.Inactive:
                case R4.Condition.ConditionClinicalStatusCodes.Resolved:
                {
                    //Act
                    var result = Mapper.Map<Condition>(source);

                    //Assert
                    result.Should().NotBeNull();
                    result.Id.Should().Be(id);
                    result.Name.Should().Be("Problem name");

                    result.AbatementPeriod.Should().NotBeNull();
                    result.AbatementPeriod.Start.Should().Be(start);
                    result.AbatementPeriod.End.Should().Be(end);

                    result.ClinicalStatus.Should().Be(expectedStatus);
                    result.RecordedDate.Should().Be(recordedDate);

                    break;
                }

                case R4.Condition.ConditionClinicalStatusCodes.Remission:
                case R4.Condition.ConditionClinicalStatusCodes.Recurrence:
                case R4.Condition.ConditionClinicalStatusCodes.Relapse:
                default:
                {
                    //Act
                    Action action = () => Mapper.Map<Condition>(source);

                    //Assert
                    var result = action.Should().ThrowExactly<ConceptNotMappedException>();
                    result.Which.Message.Should().Be("Unsupported clinical status");

                    break;
                }
            }
        }

        [Test]
        public void ConvertTest_Without_ClinicalStatus_Exception()
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            var recordedDate = new DateTimeOffset(DateTime.UtcNow);
            var start = recordedDate.AddDays(-120);
            var end = recordedDate.AddHours(24);

            var source = new R4.Condition
            {
                Id = id,
                Abatement = new FhirModel.Period
                {
                    StartElement = new FhirModel.FhirDateTime(start),
                    EndElement = new FhirModel.FhirDateTime(end)
                },
                Code = new FhirModel.CodeableConcept("system", "code", "Problem name"),
                RecordedDateElement = new FhirModel.FhirDateTime(recordedDate)
            };

            //Act
            Action action = () => Mapper.Map<Condition>(source);

            //Assert
            var result = action.Should().ThrowExactly<ConceptNotMappedException>();
            result.Which.Message.Should().Be("Can't map condition without clinical status");
        }

        [Test]
        public void ConvertTest_MoreThanOne_ClinicalStatus_Exception()
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            var recordedDate = new DateTimeOffset(DateTime.UtcNow);
            var start = recordedDate.AddDays(-120);
            var end = recordedDate.AddHours(24);

            var source = new R4.Condition
            {
                Id = id,
                Abatement = new FhirModel.Period
                {
                    StartElement = new FhirModel.FhirDateTime(start),
                    EndElement = new FhirModel.FhirDateTime(end)
                },
                ClinicalStatus = new FhirModel.CodeableConcept()
                {
                    Coding = new List<FhirModel.Coding>
                    {
                        new FhirModel.Coding
                        {
                            System = ClinicalStatusCodeSystem,
                            Version = ClinicalStatusVersion,
                            Code = R4.Condition.ConditionClinicalStatusCodes.Resolved.GetLiteral()
                        },
                        new FhirModel.Coding
                        {
                            System = ClinicalStatusCodeSystem,
                            Version = ClinicalStatusVersion,
                            Code = R4.Condition.ConditionClinicalStatusCodes.Remission.GetLiteral()
                        },
                    }
                },
                Code = new FhirModel.CodeableConcept("system", "code", "Problem name"),
                RecordedDateElement = new FhirModel.FhirDateTime(recordedDate)
            };

            //Act
            Action action = () => Mapper.Map<Condition>(source);

            //Assert
            var result = action.Should().ThrowExactly<ConceptNotMappedException>();
            result.Which.Message.Should().Be("Ambiguous codes of clinical status");
        }

        [Test]
        public void ConvertTest_Without_Abatement_Test()
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            var recordedDate = new DateTimeOffset(DateTime.UtcNow);
            var start = recordedDate.AddDays(-120);
            var end = recordedDate.AddHours(24);

            var source = new R4.Condition
            {
                Id = id,
                ClinicalStatus = new FhirModel.CodeableConcept()
                {
                    Coding = new List<FhirModel.Coding>
                    {
                        new FhirModel.Coding
                        {
                            System = ClinicalStatusCodeSystem,
                            Version = ClinicalStatusVersion,
                            Code = R4.Condition.ConditionClinicalStatusCodes.Active.GetLiteral()
                        }
                    }
                },
                Code = new FhirModel.CodeableConcept("system", "code", "Problem name"),
                RecordedDateElement = new FhirModel.FhirDateTime(recordedDate)
            };

            //Act
            var result = Mapper.Map<Condition>(source);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.Name.Should().Be("Problem name");

            result.AbatementPeriod.Should().BeNull();

            result.ClinicalStatus.Should().Be(ConditionClinicalStatus.Active);
            result.RecordedDate.Should().Be(recordedDate);
        }

        [TestCase(nameof(FhirModel.FhirDateTime))]
        [TestCase(nameof(FhirModel.Range))]
        [TestCase(nameof(FhirModel.FhirString))]
        [TestCase(nameof(R4.Age))]
        public void ConvertTest_ConvertAbatement_Exception(string fhirModelDataType)
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            var recordedDate = new DateTimeOffset(DateTime.UtcNow);
            var start = recordedDate.AddDays(-120);
            var end = recordedDate.AddHours(24);

            var source = new R4.Condition
            {
                Id = id,
                ClinicalStatus = new FhirModel.CodeableConcept()
                {
                    Coding = new List<FhirModel.Coding>
                    {
                        new FhirModel.Coding
                        {
                            System = ClinicalStatusCodeSystem,
                            Version = ClinicalStatusVersion,
                            Code = R4.Condition.ConditionClinicalStatusCodes.Resolved.GetLiteral()
                        }
                    }
                },
                Code = new FhirModel.CodeableConcept("system", "code", "Problem name"),
                RecordedDateElement = new FhirModel.FhirDateTime(recordedDate)
            };

            switch (fhirModelDataType)
            {
                case nameof(FhirModel.FhirDateTime):
                {
                    source.Abatement = new FhirModel.FhirDateTime();

                    break;
                }

                case nameof(FhirModel.Range):
                {
                    source.Abatement = new FhirModel.Range();

                    break;
                }

                case nameof(FhirModel.FhirString):
                {
                    source.Abatement = new FhirModel.FhirString();

                    break;
                }

                case nameof(R4.Age):
                {
                    source.Abatement = new R4.Age();

                    break;
                }
            }

            //Act
            Action action = () => Mapper.Map<Condition>(source);

            //Assert
            var result = action.Should().ThrowExactly<ConceptNotMappedException>();
            result.Which.Message.Should().Be("Could not map Abatement of Condition");
        }

        private ConditionClinicalStatus? CalculateConditionClinicalStatus(R4.Condition.ConditionClinicalStatusCodes clinicalStatusCode)
        {
            return clinicalStatusCode switch
            {
                R4.Condition.ConditionClinicalStatusCodes.Active => ConditionClinicalStatus.Active,
                R4.Condition.ConditionClinicalStatusCodes.Inactive => ConditionClinicalStatus.Inactive,
                R4.Condition.ConditionClinicalStatusCodes.Resolved => ConditionClinicalStatus.Resolved,
                R4.Condition.ConditionClinicalStatusCodes.Recurrence => null,
                R4.Condition.ConditionClinicalStatusCodes.Relapse => null,
                R4.Condition.ConditionClinicalStatusCodes.Remission => null,
                _ => null
            };
        }
    }
}