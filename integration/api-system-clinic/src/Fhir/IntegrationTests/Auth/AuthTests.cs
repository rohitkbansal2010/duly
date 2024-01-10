// <copyright file="AuthTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Auth;
using Duly.Clinic.Fhir.Adapter.Auth.Settings;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Fhir.Adapter.IntegrationTests.Auth
{
    [Category("IntegrationTests")]
    [TestFixture]
    public class AuthTests
    {
        [Test]
        public async Task TestGetEpicTokenURLAsync()
        {
            var config = new ConfigurationBuilder().AddUserSecrets<AuthTests>().Build();
            var apiClientAuthSettings = config.GetSection("DulyRestApiClient").Get<DulyRestApiClientAuthSettings>();
            var fhirAuthSettings = config.GetSection("Fhir").Get<FhirAuthSettings>();
            IOptionsMonitor<DulyRestApiClientAuthSettings> dulyRestApiClientAuthSettingsOptionsMonitor = new TestDulyRestApiClientAuthSettingsOptionsMonitor(apiClientAuthSettings);
            IOptionsMonitor<FhirAuthSettings> fhirAuthSettingsOptionsMonitor = new TestFhirAuthSettingsOptionsMonitor(fhirAuthSettings);

            var clientAssertionCreator = new ClientAssertionCreator(dulyRestApiClientAuthSettingsOptionsMonitor, fhirAuthSettingsOptionsMonitor, null);

            var clientAssertion = await clientAssertionCreator.GetClientAssertionAsync();
            var jsonToken = await clientAssertionCreator.AuthenticationAsync(clientAssertion);

            jsonToken.AccessToken.Should().NotBeEmpty();

            jsonToken.ExpiresIn.Should().Be(3600);
        }

        public class TestDulyRestApiClientAuthSettingsOptionsMonitor : IOptionsMonitor<DulyRestApiClientAuthSettings>
        {
            public TestDulyRestApiClientAuthSettingsOptionsMonitor(DulyRestApiClientAuthSettings apiClientAuthSettings)
            {
                CurrentValue = apiClientAuthSettings;
            }

            public DulyRestApiClientAuthSettings CurrentValue { get; }

            public DulyRestApiClientAuthSettings Get(string name)
            {
                throw new NotImplementedException();
            }

            public IDisposable OnChange(Action<DulyRestApiClientAuthSettings, string> listener)
            {
                throw new NotImplementedException();
            }
        }

        public class TestFhirAuthSettingsOptionsMonitor : IOptionsMonitor<FhirAuthSettings>
        {
            public TestFhirAuthSettingsOptionsMonitor(FhirAuthSettings apiClientAuthSettings)
            {
                CurrentValue = apiClientAuthSettings;
            }

            public FhirAuthSettings CurrentValue { get; }

            public FhirAuthSettings Get(string name)
            {
                throw new NotImplementedException();
            }

            public IDisposable OnChange(Action<FhirAuthSettings, string> listener)
            {
                throw new NotImplementedException();
            }
        }
    }
}