// <copyright file="DiagnosticReportConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Api.Repositories.Mappings.ObservationConverters;
using Duly.Clinic.Api.Tests.Common;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts;
using FluentAssertions;
using Hl7.Fhir.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Quantity = Hl7.Fhir.Model.Quantity;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class DiagnosticReportConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        private const string SystemObservationInterpretation = "http://hl7.org/fhir/ValueSet/observation-interpretation";

        [Test]
        public void TestConvert()
        {
            var expectedDate = new DateTimeOffset(2020, 12, 12, 12, 0, 0, TimeSpan.Zero);
            var reportWithCompartments = new DiagnosticReportWithCompartments
            {
                Resource = new R4.DiagnosticReport
                {
                    Id = "TestId",
                    Status = R4.DiagnosticReport.DiagnosticReportStatus.Final,
                    Code = new CodeableConcept("some", "other", "display", "text"),
                    Effective = new FhirDateTime(2020, 12, 12),
                    Issued = expectedDate
                },
                Observations = new R4.Observation[]
                {
                    new()
                    {
                        Id = "ObservationId",
                        Code = new CodeableConcept(default, "test-code"),
                        Effective = new FhirDateTime(2020, 12, 12),
                        Interpretation = new List<CodeableConcept>
                        {
                            new(SystemObservationInterpretation, "code", "display", "Some Text")
                        },
                        ReferenceRange = new List<R4.Observation.ReferenceRangeComponent>()
                        {
                            new()
                            {
                                Low = new Hl7.Fhir.Model.Quantity(1.1M, "some"),
                                High = new Hl7.Fhir.Model.Quantity(1.2M, "some"),
                                Text = "Text of Range"
                            }
                        },
                        Value = new Hl7.Fhir.Model.Quantity(1.13M, "some"),
                    },
                    new()
                    {
                        Id = "ObservationId2",
                        Code = new CodeableConcept(default, "test-code"),
                        Effective = new FhirDateTime(2020, 12, 12),
                        Interpretation = new List<CodeableConcept>
                        {
                            new(SystemObservationInterpretation, "code", "display", "Some Text")
                        },
                        ReferenceRange = new List<R4.Observation.ReferenceRangeComponent>()
                        {
                            new()
                            {
                                Low = new Hl7.Fhir.Model.Quantity(1.1M, "some"),
                                High = new Hl7.Fhir.Model.Quantity(1.2M, "some"),
                                Text = "Text of Range"
                            }
                        },
                        Value = new CodeableConcept("system in value", "code in value", "display in value", "text in value")
                    },
                    new()
                    {
                        Id = "ObservationId3",
                        Code = new CodeableConcept(default, "test-code"),
                        Effective = new FhirDateTime(2020, 12, 12),
                        Interpretation = new List<CodeableConcept>
                        {
                            new(SystemObservationInterpretation, "code", "display", "Some Text")
                        },
                        ReferenceRange = new List<R4.Observation.ReferenceRangeComponent>()
                        {
                            new()
                            {
                                Low = new Hl7.Fhir.Model.Quantity(1.1M, "some"),
                                High = new Hl7.Fhir.Model.Quantity(1.2M, "some"),
                                Text = "Text of Range"
                            }
                        },
                        Value = new FhirString("Some hot string"),
                        Note = new List<R4.Annotation>
                        {
                            new() { Text = new(@"Desirable <100 mg/dL \r\nBorderline 100-129 mg/dL \r\nHigh     >=130mg/dL\r\n\r\n") },
                            new() { Text = new("test-text-note") }
                        }
                    }
                },
                Performers = new PractitionerWithRoles[]
                {
                    new()
                    {
                        Resource = new R4.Practitioner
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = new List<R4.HumanName>
                            {
                                new()
                                {
                                    Use = R4.HumanName.NameUse.Official,
                                }
                            }
                        },
                        Roles = Array.Empty<R4.PractitionerRole>()
                    }
                }
            };

            var report = Mapper.Map<DiagnosticReport>(reportWithCompartments);

            report.Status.Should().Be(DiagnosticReportStatus.Final);
            report.Name.Should().Be(reportWithCompartments.Resource.Code.Text);
            report.Performers.First().Id.Should().Be(reportWithCompartments.Performers.First().Resource.Id);
            report.EffectiveDate.Should().Be(expectedDate);
            report.Issued.Should().Be(expectedDate);

            report.Observations.Length.Should().Be(3);
            report.Observations[0].Id.Should().Be(reportWithCompartments.Observations[0].Id);
            report.Observations[0].ComponentName.Should().Be(reportWithCompartments.Observations[0].Code.Text);
            report.Observations[0].Date.Should().Be(expectedDate);
            report.Observations[0].Components[0].Interpretations[0].Code.Should()
                .Be(reportWithCompartments.Observations[0].Interpretation[0].Coding
                    .First(c => c.System.Equals(SystemObservationInterpretation, StringComparison.OrdinalIgnoreCase)).Code);
            report.Observations[0].Components[0].Interpretations[0].Text.Should()
                .Be(reportWithCompartments.Observations[0].Interpretation[0].Text);
            report.Observations[0].Components[0].ReferenceRange[0].Text.Should().Be(reportWithCompartments.Observations[0].ReferenceRange[0].Text);
            report.Observations[0].Components[0].Measurements[0].Value.Should()
                .Be(((Quantity)reportWithCompartments.Observations[0].Value).Value);
            report.Observations[0].Components[0].Measurements[0].Unit.Should()
                .Be(((Quantity)reportWithCompartments.Observations[0].Value).Unit);

            report.Observations[1].Id.Should().Be(reportWithCompartments.Observations[1].Id);
            report.Observations[1].ComponentName.Should().Be(reportWithCompartments.Observations[1].Code.Text);
            report.Observations[1].Date.Should().Be(expectedDate);
            report.Observations[1].Components[0].Interpretations[0].Code.Should()
                .Be(reportWithCompartments.Observations[1].Interpretation[0].Coding
                    .First(c => c.System.Equals(SystemObservationInterpretation, StringComparison.OrdinalIgnoreCase)).Code);
            report.Observations[1].Components[0].Interpretations[0].Text.Should()
                .Be(reportWithCompartments.Observations[1].Interpretation[0].Text);
            report.Observations[1].Components[0].ReferenceRange[0].Text.Should().Be(reportWithCompartments.Observations[1].ReferenceRange[0].Text);
            report.Observations[1].Components[0].ValueText.Should().Be(((CodeableConcept)reportWithCompartments.Observations[1].Value).Text);

            report.Observations[2].Id.Should().Be(reportWithCompartments.Observations[2].Id);
            report.Observations[2].ComponentName.Should().Be(reportWithCompartments.Observations[2].Code.Text);
            report.Observations[2].Date.Should().Be(expectedDate);
            report.Observations[2].Components[0].Interpretations[0].Code.Should()
                .Be(reportWithCompartments.Observations[2].Interpretation[0].Coding
                    .First(c => c.System.Equals(SystemObservationInterpretation, StringComparison.OrdinalIgnoreCase)).Code);
            report.Observations[2].Components[0].Interpretations[0].Text.Should()
                .Be(reportWithCompartments.Observations[2].Interpretation[0].Text);
            report.Observations[2].Components[0].ReferenceRange[0].Text.Should().Be(reportWithCompartments.Observations[2].ReferenceRange[0].Text);
            report.Observations[2].Components[0].ReferenceRange[0].High.Value.Should()
                .Be(reportWithCompartments.Observations[2].ReferenceRange[0].High.Value);
            report.Observations[2].Components[0].ReferenceRange[0].Low.Value.Should()
                .Be(reportWithCompartments.Observations[2].ReferenceRange[0].Low.Value);
            report.Observations[2].Components[0].ValueText.Should().Be(((FhirString)reportWithCompartments.Observations[2].Value).Value);
            report.Observations[2].Components[0].Notes[0].Should()
                .Be(reportWithCompartments.Observations[2].Note[0].Text.Value);
        }

        [Test]
        [TestCase(R4.DiagnosticReport.DiagnosticReportStatus.Amended, DiagnosticReportStatus.Amended)]
        [TestCase(R4.DiagnosticReport.DiagnosticReportStatus.Appended, DiagnosticReportStatus.Appended)]
        [TestCase(R4.DiagnosticReport.DiagnosticReportStatus.Cancelled, DiagnosticReportStatus.Cancelled)]
        [TestCase(R4.DiagnosticReport.DiagnosticReportStatus.Corrected, DiagnosticReportStatus.Corrected)]
        [TestCase(R4.DiagnosticReport.DiagnosticReportStatus.EnteredInError, DiagnosticReportStatus.EnteredInError)]
        [TestCase(R4.DiagnosticReport.DiagnosticReportStatus.Partial, DiagnosticReportStatus.Partial)]
        [TestCase(R4.DiagnosticReport.DiagnosticReportStatus.Preliminary, DiagnosticReportStatus.Preliminary)]
        [TestCase(R4.DiagnosticReport.DiagnosticReportStatus.Registered, DiagnosticReportStatus.Registered)]
        [TestCase(R4.DiagnosticReport.DiagnosticReportStatus.Unknown, DiagnosticReportStatus.Unknown)]
        public void TestConvert_WithDiffferent_Statuses(R4.DiagnosticReport.DiagnosticReportStatus status, DiagnosticReportStatus expected)
        {
            var expectedDate = new DateTimeOffset(2020, 12, 12, 0, 0, 0, TimeSpan.Zero);
            var reportWithCompartments = new DiagnosticReportWithCompartments
            {
                Resource = new R4.DiagnosticReport
                {
                    Id = "TestId",
                    Status = status,
                    Code = new CodeableConcept("some", "other", "display", "text"),
                    Effective = new FhirDateTime(2020, 12, 12),
                    Issued = expectedDate
                },
                Observations = new R4.Observation[]
                {
                },
                Performers = new PractitionerWithRoles[]
                {
                    new()
                    {
                        Resource = new R4.Practitioner
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = new List<R4.HumanName>
                            {
                                new()
                                {
                                    Use = R4.HumanName.NameUse.Official,
                                }
                            }
                        },
                        Roles = Array.Empty<R4.PractitionerRole>()
                    }
                }
            };

            var report = Mapper.Map<DiagnosticReport>(reportWithCompartments);

            report.Status.Should().Be(expected);
        }

        [Test]
        public void TestConvert_UnsupportedStatus_ShouldThrow()
        {
            var expectedDate = new DateTimeOffset(2020, 12, 12, 0, 0, 0, TimeSpan.Zero);
            var reportWithCompartments = new DiagnosticReportWithCompartments
            {
                Resource = new R4.DiagnosticReport
                {
                    Id = "TestId",
                    Status = (R4.DiagnosticReport.DiagnosticReportStatus)10,
                    Code = new CodeableConcept("some", "other", "display", "text"),
                    Effective = new FhirDateTime(2020, 12, 12),
                    Issued = expectedDate
                },
                Observations = new R4.Observation[]
                {
                },
                Performers = new PractitionerWithRoles[]
                {
                    new()
                    {
                        Resource = new R4.Practitioner
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = new List<R4.HumanName>
                            {
                                new()
                                {
                                    Use = R4.HumanName.NameUse.Official,
                                }
                            }
                        },
                        Roles = Array.Empty<R4.PractitionerRole>()
                    }
                }
            };

            Mapper.Invoking(x => x.Map<DiagnosticReport>(reportWithCompartments)).Should()
                .Throw<ConceptNotMappedException>();
        }

        [Test]
        public void Test_Throws()
        {
            var converter = new TypedObservationConverterLabResult();

            converter.Invoking(x => x.ObservationType).Should().Throw<NotSupportedException>();
            converter.Invoking(x => x.SupportedCodings).Should().Throw<NotSupportedException>();
        }
    }
}