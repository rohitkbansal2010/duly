// <copyright file="FhirAdapterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;
extern alias stu3;
using Duly.Clinic.Fhir.Adapter.Adapters.Implementations;
using Duly.Clinic.Fhir.Adapter.Builders.Implementations;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Extensions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using R4 = r4::Hl7.Fhir;
using STU3 = stu3::Hl7.Fhir;

namespace Fhir.Adapter.IntegrationTests.Repositories.Implementations
{
    [Category("IntegrationTests")]
    [TestFixture]
    public class FhirAdapterTests
    {
        private const string Endpoint = "https://spark.incendi.no/fhir";

        private static readonly Dictionary<string, string> _inMemorySettings = new()
        {
            { "TmpEncountersDesiredDate", DateTime.Today.ToString(CultureInfo.InvariantCulture) }
        };

        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(_inMemorySettings)
            .Build();

        private FhirClientWrapperR4 _clientR4;
        private FhirClientWrapperStu3 _clientSTU3;
        private Mock<ILogger<PractitionerWithRolesBuilder>> _loggerMock;
        private Mock<IPrivateEpicCall> _privateEpicCall;
        [SetUp]
        public void SetUp()
        {
            _clientR4 = new FhirClientWrapperR4(new R4.Rest.FhirClient(Endpoint));
            _clientSTU3 = new FhirClientWrapperStu3(new STU3.Rest.FhirClient(Endpoint));
            _loggerMock = new Mock<ILogger<PractitionerWithRolesBuilder>>();
            _privateEpicCall = new Mock<IPrivateEpicCall>();
        }

        [Test]
        public async Task GetResourceWithCompartmentsAsyncTest()
        {
            var practitionerBuilder = new PractitionerWithRolesBuilder(_clientR4, _clientSTU3, _loggerMock.Object);
            var adapter = new PractitionerWithCompartmentsAdapter(_clientR4, practitionerBuilder);

            var result = await adapter.FindPractitionersWithRolesAsync(new SearchCriteria { SiteId = "1" });

            result.Should().HaveCountGreaterThan(0);
        }

        [Test]
        public async Task GetEncountersWithCompartmentsAsyncTest()
        {
            var practitionerBuilder = new PractitionerWithRolesBuilder(_clientR4, _clientSTU3, _loggerMock.Object);
            var adapter = new EncounterWithCompartmentsAdapter(_clientR4, null, new EncounterWithCompartmentsBuilder(_clientR4, practitionerBuilder), _configuration);

            var result = await adapter.FindEncountersWithCompartmentsAsync(new SearchCriteria { StartDateTime = new DateTime(2015, 1, 17) });

            result.Should().HaveCountGreaterThan(0);
        }

        [Test]
        public async Task GetEncountersWithParticipantsAsyncTest()
        {
            var practitionerBuilder = new PractitionerWithRolesBuilder(_clientR4, _clientSTU3, _loggerMock.Object);
            var adapter = new EncounterWithCompartmentsAdapter(_clientR4, null, new EncounterWithCompartmentsBuilder(_clientR4, practitionerBuilder), _configuration);

            var result = await adapter.FindEncounterWithParticipantsAsync("43");

            result.Should().NotBeNull();
        }

        [Test]
        public async Task GetPatientWithCompartmentsAsyncTest()
        {
            var client = new FhirClientWrapperR4(new R4.Rest.FhirClient(Endpoint));
            var adapter = new PatientWithCompartmentsAdapter(client, new PatientWithCompartmentsBuilder(), _privateEpicCall.Object);

            var result = await adapter.FindPatientByIdAsync("f001");

            result.Should().NotBeNull();
        }

        [Test]
        public async Task GetMedicationWithCompartmentsAsyncTest()
        {
            var practitionerBuilder = new PractitionerWithRolesBuilder(_clientR4, _clientSTU3, _loggerMock.Object);
            var adapter = new MedicationStatementWithCompartmentsAdapter(_clientSTU3, _clientR4, new MedicationStatementWithCompartmentsBuilder(), new MedicationRequestWithCompartmentsBuilder(), practitionerBuilder);
            var criteria = new MedicationSearchCriteria { PatientId = "pat1", Status = "Active" };

            var result = await adapter.FindMedicationsWithCompartmentsAsync(criteria);

            result.Should().NotBeNull();
        }

        [Test]
        public async Task GetObservationWithCompartmentsAsyncTest()
        {
            var adapter = new ObservationWithCompartmentsAdapter(_clientR4, new ObservationWithCompartmentsBuilder());
            var criteria = new ObservationSearchCriteria() { PatientId = "pat1" };

            var result = await adapter.FindObservationsWithCompartmentsAsync(criteria);

            result.Should().NotBeNull();
        }
    }
}