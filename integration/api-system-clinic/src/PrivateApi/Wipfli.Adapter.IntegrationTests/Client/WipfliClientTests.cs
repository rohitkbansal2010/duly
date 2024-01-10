// <copyright file="WipfliClientTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Wipfli.Adapter.Client;

namespace Wipfli.Adapter.IntegrationTests.Client
{
    [TestFixture]
    public class WipfliClientTests
    {
        [Test]
        public async Task TestScheduleDaysForProvider()
        {
            string keyVaultUrl = "https://duly-d-certs-kv.vault.azure.net/";
            var client = new CertificateClient(vaultUri: new Uri(keyVaultUrl), credential: new DefaultAzureCredential());
            var certificate = await client.DownloadCertificateAsync("digital-np-epic-bridge-cert");

            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(certificate);
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://npd.dupagemedicalgroup.com:447"),
            };

            string username = "emp$28281";
            string password = "testing1";

            string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", svcCredentials);
            httpClient.DefaultRequestHeaders.Add("Epic-Client-ID", "ef6de80d-1de6-4e75-acbf-46abd721ce6b");

            var body = new ScheduleDaysForProviderSearchCriteria
            {
                DepartmentIDs = new Identity[]
                {
                    new()
                    {
                        ID = "25069",
                        Type = "External"
                    }
                },
                VisitTypeIDs = new Identity[]
                {
                    new()
                    {
                        ID = "212",
                        Type = "External"
                    }
                },
                ProviderID = "1405",
                ProviderIDType = "External",
                StartDate = DateTime.Now.AddDays(2).Date,
                EndDate = DateTime.Now.AddDays(9).Date
            };

            IWipfliClient wipfli = new WipfliClient(httpClient);

            var result = await wipfli.GetScheduleDaysForProvider(body);

            result.Should().NotBeNull();
        }

        [Test]
        public async Task TestScheduleAppointmentWithInsurance()
        {
            string keyVaultUrl = "https://duly-d-certs-kv.vault.azure.net/";
            var client = new CertificateClient(vaultUri: new Uri(keyVaultUrl), credential: new DefaultAzureCredential());
            var certificate = await client.DownloadCertificateAsync("digital-np-epic-bridge-cert");

            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(certificate);
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://npd.dupagemedicalgroup.com:447"),
            };

            string username = "emp$28281";
            string password = "testing1";

            string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", svcCredentials);
            httpClient.DefaultRequestHeaders.Add("Epic-Client-ID", "ef6de80d-1de6-4e75-acbf-46abd721ce6b");

            var request = new ScheduleAppointmentWithInsuranceRequest
            {
                PatientID = "7650074",
                PatientIDType = "EXTERNAL",
                DepartmentID = "25069",
                DepartmentIDType = "External",
                VisitTypeID = "2147",
                VisitTypeIDType = "External",
                Date = DateTime.Now.AddDays(5).Date,
                IsReviewOnly = true,
                ProviderID = "1405",
                ProviderIDType = "External",
                Time = TimeSpan.Parse("14:15:00")
            };

            IWipfliClient wipfli = new WipfliClient(httpClient);

            var result = await wipfli.ScheduleAppointmentWithInsurance(request);

            result.Should().NotBeNull();
        }
    }
}