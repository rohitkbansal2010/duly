// <copyright file="SiteServiceTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Configurations;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Services.Implementations;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Site = Duly.CollaborationView.Encounter.Api.Contracts.Site;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations
{
    [TestFixture]
    public class SiteServiceTests
    {
        private readonly IEnumerable<Site> _sites = new Site[]
        {
            new()
            {
                Id = "628",
                Address = new Address()
                {
                    Line = "1121 South Blvd",
                    City = "Oak Park",
                    State = "Illinois",
                    PostalCode = "60302"
                }
            },
            new()
            {
                Id = "627",
                Address = new Address()
                {
                    Line = "9233 W. 159th St.",
                    City = "Orland Hills",
                    State = "Illinois",
                    PostalCode = "60487"
                }
            },
            new()
            {
                Id = "629",
                Address = new Address()
                {
                    Line = "1034 N. Rohlwing Rd.",
                    City = "Oak Park",
                    State = "Illinois",
                    PostalCode = "60302"
                }
            }
        };

        private SiteDataOptions _siteDataOptions;
        private IOptionsMonitor<SiteDataOptions> _optionsMonitorMock;
        private Mock<ISitesRepository> _siteRepository;
        private Mock<IMapper> _mapper;

        public static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData("1", null);
            yield return new TestCaseData("629", new Site()
            {
                Id = "629",
                Address = new Address()
                {
                    Line = "1034 N. Rohlwing Rd.",
                    City = "Oak Park",
                    State = "Illinois",
                    PostalCode = "60302"
                }
            });
        }

        [SetUp]
        public void SetUp()
        {
            _siteDataOptions = new SiteDataOptions { Sites = JsonConvert.SerializeObject(_sites) };
            _optionsMonitorMock = Mock.Of<IOptionsMonitor<SiteDataOptions>>(_ => _.CurrentValue == _siteDataOptions);
            _mapper = new Mock<IMapper>();
            _siteRepository = new Mock<ISitesRepository>();
        }

        //[Test]
        //public async Task GetSitesAsyncTest()
        //{
        //    var serviceMocked = new SiteService(_optionsMonitorMock, _siteRepository.Object, _mapper.Object);

        //    //Act
        //    var results = await serviceMocked.GetSitesAsync();

        //    //Assert
        //    //results.Should().NotBeNullOrEmpty();
        //    results.Should().AllBeOfType<Site>();
        //    results.Should().BeEquivalentTo(_sites);
        //}

        //[TestCaseSource(nameof(TestCases))]
        //public async Task GetSitesAsyncTest(string id, Site expected)
        //{
        //    var serviceMocked = new SiteService(_optionsMonitorMock, _siteRepository.Object, _mapper.Object);

        //    //Act
        //    var results = await serviceMocked.GetSiteAsync(id);

        //    //Assert
        //    results.Should().BeEquivalentTo(expected);
        //}
    }
}
