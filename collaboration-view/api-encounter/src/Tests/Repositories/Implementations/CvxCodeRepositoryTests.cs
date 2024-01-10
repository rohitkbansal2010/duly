// <copyright file="CvxCodeRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.CdcClient.Interfaces;
using Duly.CollaborationView.CdcClient.Models;
using Duly.CollaborationView.Encounter.Api.Configurations;
using Duly.CollaborationView.Encounter.Api.Repositories.Implementations;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.Common.Cache.Clients.Interfaces;
using Duly.Common.Cache.Helpers.Implementations;
using Duly.Common.Cache.Helpers.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class CvxCodeRepositoryTests
    {
        private const string ContentModel = "CDC_IMMUN_VAC_MODEL";
        private const string CvxCodeAlias = "CvxCode";
        private const string RelationshipMnemonic = "IS_MEMBER_OF";

        private ICacheKeyBuilder _cacheKeyBuilder;
        private Mock<IOptionsMonitor<CdcApiOptions>> _cdcApiOptionsMonitorMock;
        private Mock<ICdcApiClient> _cdcApiClientMock;
        private Mock<ICacheClient> _cacheClientMock;
        private Mock<ILogger<CvxCodeRepository>> _loggerMock;

        [SetUp]
        public void SetUp()
        {
            ConfigureLogger();
            ConfigureCacheKeyBuilder();
            ConfigureCdcApiOptions();
            _cacheClientMock = new Mock<ICacheClient>();
            _cdcApiClientMock = new Mock<ICdcApiClient>();
        }

        [Test]
        public async Task FindVaccineGroupNamesByCodesAsync_Test_NotFoundImmunizationCdcCatalogs()
        {
            //Arrange
            var cvxCodes = new string[]
            {
                "110", "115", "197", "208"
            };

            var immunizationCdcCatalogs = BuildImmunizationCdcCatalogs(isEmptyCatalogs: true);
            var expectedCvxCodeVaccineGroupNameDictionary = BuildCvxCodeVaccineGroupNameDictionary(cvxCodes);

            var repository = new CvxCodeRepository(
                _cdcApiClientMock.Object,
                _cdcApiOptionsMonitorMock.Object,
                _cacheKeyBuilder,
                _cacheClientMock.Object,
                _loggerMock.Object);

            //Act
            var result = await repository.FindVaccineGroupNamesByCodesAsync(cvxCodes);

            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                    It.Is<It.IsAnyType>(
                        (obj, t) => obj.ToString().StartsWith("CDC REST API client does not find the required catalogs.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

            result.Should().NotBeNull();
            result.Keys.Count().Should().Be(2);
        }

        [Test]
        public async Task FindVaccineGroupNamesByCodesAsync_Test_WithOutCodes()
        {
            //Arrange
            var cvxCodes = default(string[]);

            var immunizationCdcCatalogs = BuildImmunizationCdcCatalogs();
            var expectedCvxCodeVaccineGroupNameDictionary = BuildCvxCodeVaccineGroupNameDictionary(cvxCodes);

            var repository = new CvxCodeRepository(
                _cdcApiClientMock.Object,
                _cdcApiOptionsMonitorMock.Object,
                _cacheKeyBuilder,
                _cacheClientMock.Object,
                _loggerMock.Object);

            //Act
            var result = await repository.FindVaccineGroupNamesByCodesAsync(cvxCodes);

            result.Should().NotBeNull();
            result.Keys.Count().Should().Be(0);
        }

        [Test]
        public async Task FindVaccineGroupNamesByCodesAsync_Test()
        {
            //Arrange
            var cvxCodes = new string[]
            {
                "110", "115", "197", "withoutGroupName"
            };

            var immunizationCdcCatalogs = BuildImmunizationCdcCatalogs();
            var expectedCvxCodeVaccineGroupNameDictionary = BuildCvxCodeVaccineGroupNameDictionary(cvxCodes);

            var repository = new CvxCodeRepository(
                _cdcApiClientMock.Object,
                _cdcApiOptionsMonitorMock.Object,
                _cacheKeyBuilder,
                _cacheClientMock.Object,
                _loggerMock.Object);

            //Act
            var result = await repository.FindVaccineGroupNamesByCodesAsync(cvxCodes);

            result.Should().NotBeNull();
            result.Keys.Count().Should().Be(3);
        }

        [Test]
        public async Task FindVaccineGroupNamesByCodesAsync_Test_ImmunizationCdcCatalogsCached()
        {
            //Arrange
            var cvxCodes = new string[]
            {
                "110", "115", "197", "withoutGroupName"
            };

            var immunizationCdcCatalogs = BuildImmunizationCdcCatalogs(isCached: true);
            var expectedCvxCodeVaccineGroupNameDictionary = BuildCvxCodeVaccineGroupNameDictionary(cvxCodes);

            var repository = new CvxCodeRepository(
                _cdcApiClientMock.Object,
                _cdcApiOptionsMonitorMock.Object,
                _cacheKeyBuilder,
                _cacheClientMock.Object,
                _loggerMock.Object);

            //Act
            var result = await repository.FindVaccineGroupNamesByCodesAsync(cvxCodes);

            result.Should().NotBeNull();
            result.Keys.Count().Should().Be(3);
        }

        [Test]
        public async Task FindVaccineGroupNamesByCodesAsync_Test_AllCodesCached()
        {
            //Arrange
            var cvxCodes = new string[]
            {
                "110"
            };

            var immunizationCdcCatalogs = BuildImmunizationCdcCatalogs();
            var expectedCvxCodeVaccineGroupNameDictionary = BuildCvxCodeVaccineGroupNameDictionary(cvxCodes);

            var repository = new CvxCodeRepository(
                _cdcApiClientMock.Object,
                _cdcApiOptionsMonitorMock.Object,
                _cacheKeyBuilder,
                _cacheClientMock.Object,
                _loggerMock.Object);

            //Act
            var result = await repository.FindVaccineGroupNamesByCodesAsync(cvxCodes);

            result.Should().NotBeNull();
            result.Keys.Count().Should().Be(1);
        }

        [Test]
        public async Task FindVaccineGroupNamesByCodesAsync_Test_GetImmunizationCdcCatalogs_CacheClient_GetError()
        {
            //Arrange
            var cvxCodes = new string[]
            {
                "110", "115", "197", "withoutGroupName"
            };

            var immunizationCdcCatalogs = BuildImmunizationCdcCatalogs();
            var expectedCvxCodeVaccineGroupNameDictionary = BuildCvxCodeVaccineGroupNameDictionary(cvxCodes);

            var exceptionMessage = "CacheClientException";
            _cacheClientMock
                .Setup(x => x.GetAsync<ImmunizationCdcCatalogs>(It.IsAny<string>()))
                .Throws(new Exception(exceptionMessage));

            var repository = new CvxCodeRepository(
                _cdcApiClientMock.Object,
                _cdcApiOptionsMonitorMock.Object,
                _cacheKeyBuilder,
                _cacheClientMock.Object,
                _loggerMock.Object);

            //Act
            var result = await repository.FindVaccineGroupNamesByCodesAsync(cvxCodes);

            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                    It.Is<It.IsAnyType>(
                        (obj, t) => obj.ToString().StartsWith("The Redis Cache client exited with an error:")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public async Task FindVaccineGroupNamesByCodesAsync_Test_GetImmunizationCdcCatalogs_CacheClient_AddError()
        {
            //Arrange
            var cvxCodes = new string[]
            {
                "110", "115", "197", "withoutGroupName"
            };

            var immunizationCdcCatalogs = BuildImmunizationCdcCatalogs();
            var expectedCvxCodeVaccineGroupNameDictionary = BuildCvxCodeVaccineGroupNameDictionary(cvxCodes);

            var exceptionMessage = "CacheClientException";
            _cacheClientMock
                .Setup(x => x.AddAsync<ImmunizationCdcCatalogs>(
                    It.IsAny<string>(),
                    It.IsAny<ImmunizationCdcCatalogs>(),
                    It.IsAny<DateTimeOffset?>()))
                .Throws(new Exception(exceptionMessage));

            var repository = new CvxCodeRepository(
                _cdcApiClientMock.Object,
                _cdcApiOptionsMonitorMock.Object,
                _cacheKeyBuilder,
                _cacheClientMock.Object,
                _loggerMock.Object);

            //Act
            var result = await repository.FindVaccineGroupNamesByCodesAsync(cvxCodes);

            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                    It.Is<It.IsAnyType>(
                        (obj, t) => obj.ToString().StartsWith("The Redis Cache client exited with an error:")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                    It.Is<It.IsAnyType>(
                        (obj, t) => obj.ToString().StartsWith("Redis Cache client failed to store the value for key:")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.AtLeastOnce);
        }

        [Test]
        public async Task FindVaccineGroupNamesByCodesAsync_Test_GetImmunizationCdcCatalogs_ApiClientError()
        {
            //Arrange
            var cvxCodes = new string[]
            {
                "110", "115", "197", "withoutGroupName"
            };

            var immunizationCdcCatalogs = BuildImmunizationCdcCatalogs();
            var expectedCvxCodeVaccineGroupNameDictionary = BuildCvxCodeVaccineGroupNameDictionary(cvxCodes);

            var exceptionMessage = "ApiClientException";
            _cdcApiClientMock
                .Setup(x => x.GetContentModelCatalogsAsync(It.IsAny<string>()))
                .Throws(new Exception(exceptionMessage));

            var repository = new CvxCodeRepository(
                _cdcApiClientMock.Object,
                _cdcApiOptionsMonitorMock.Object,
                _cacheKeyBuilder,
                _cacheClientMock.Object,
                _loggerMock.Object);

            //Act
            var result = await repository.FindVaccineGroupNamesByCodesAsync(cvxCodes);

            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                    It.Is<It.IsAnyType>(
                        (obj, t) => obj.ToString().StartsWith("CDC REST API client exited with an error:")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public async Task FindVaccineGroupNamesByCodesAsync_Test_CacheClient_GetError()
        {
            //Arrange
            var cvxCodes = new string[]
            {
                "110", "115", "197", "withoutGroupName"
            };

            var immunizationCdcCatalogs = BuildImmunizationCdcCatalogs();
            var expectedCvxCodeVaccineGroupNameDictionary = BuildCvxCodeVaccineGroupNameDictionary(cvxCodes);

            var exceptionMessage = "CacheClientException";
            _cacheClientMock
                .Setup(x => x.GetAsync<string>(It.IsAny<string>()))
                .Throws(new Exception(exceptionMessage));

            var repository = new CvxCodeRepository(
                _cdcApiClientMock.Object,
                _cdcApiOptionsMonitorMock.Object,
                _cacheKeyBuilder,
                _cacheClientMock.Object,
                _loggerMock.Object);

            //Act
            var result = await repository.FindVaccineGroupNamesByCodesAsync(cvxCodes);

            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                    It.Is<It.IsAnyType>(
                        (obj, t) => obj.ToString().StartsWith("The Redis Cache client exited with an error:")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.AtLeastOnce);
        }

        [Test]
        public async Task FindVaccineGroupNamesByCodesAsync_Test_CacheClient_AddError()
        {
            //Arrange
            var cvxCodes = new string[]
            {
                "110", "115", "197", "withoutGroupName"
            };

            var immunizationCdcCatalogs = BuildImmunizationCdcCatalogs();
            var expectedCvxCodeVaccineGroupNameDictionary = BuildCvxCodeVaccineGroupNameDictionary(cvxCodes);

            var exceptionMessage = "CacheClientException";
            _cacheClientMock
                .Setup(x => x.AddAsync<string>(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset?>()))
                .Throws(new Exception(exceptionMessage));

            var repository = new CvxCodeRepository(
                _cdcApiClientMock.Object,
                _cdcApiOptionsMonitorMock.Object,
                _cacheKeyBuilder,
                _cacheClientMock.Object,
                _loggerMock.Object);

            //Act
            var result = await repository.FindVaccineGroupNamesByCodesAsync(cvxCodes);

            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                    It.Is<It.IsAnyType>(
                        (obj, t) => obj.ToString().StartsWith("The Redis Cache client exited with an error:")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.AtLeastOnce);

            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                    It.Is<It.IsAnyType>(
                        (obj, t) => obj.ToString().StartsWith("Redis Cache client failed to store the value for key:")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.AtLeastOnce);
        }

        [Test]
        public async Task FindVaccineGroupNamesByCodesAsync_Test_ApiClientError()
        {
            //Arrange
            var cvxCodes = new string[]
            {
                "110", "115", "197", "withoutGroupName"
            };

            var immunizationCdcCatalogs = BuildImmunizationCdcCatalogs();
            var expectedCvxCodeVaccineGroupNameDictionary = BuildCvxCodeVaccineGroupNameDictionary(cvxCodes);

            var exceptionMessage = "ApiClientException";
            _cdcApiClientMock
                .Setup(x => x.FindContentModelCatalogRelationsByCodesAsync(
                    ContentModel,
                    "CvxCatalogUID",
                    It.IsAny<string[]>()))
                .Throws(new Exception(exceptionMessage));

            var repository = new CvxCodeRepository(
                _cdcApiClientMock.Object,
                _cdcApiOptionsMonitorMock.Object,
                _cacheKeyBuilder,
                _cacheClientMock.Object,
                _loggerMock.Object);

            //Act
            var result = await repository.FindVaccineGroupNamesByCodesAsync(cvxCodes);

            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                    It.Is<It.IsAnyType>(
                        (obj, t) => obj.ToString().StartsWith("CDC REST API client exited with an error:")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        private void ConfigureLogger()
        {
            _loggerMock = new Mock<ILogger<CvxCodeRepository>>();
        }

        private void ConfigureCdcApiOptions()
        {
            _cdcApiOptionsMonitorMock = new Mock<IOptionsMonitor<CdcApiOptions>>();
            _cdcApiOptionsMonitorMock
                .Setup(x => x.CurrentValue)
                .Returns(new CdcApiOptions
                {
                    ContentModel = ContentModel,
                    CatalogCVX = "CDC_IMMUN_CVX",
                    CatalogVaccineGroup = "CDC_IMMUN_VAC_GRP"
                });
        }

        private void ConfigureCacheKeyBuilder()
        {
            _cacheKeyBuilder = new DefaultCacheKeyBuilder();
        }

        private ImmunizationCdcCatalogs BuildImmunizationCdcCatalogs(bool isCached = false, bool isEmptyCatalogs = false)
        {
            var immunizationCdcCatalogs = default(ImmunizationCdcCatalogs);

            if (isCached)
            {
                immunizationCdcCatalogs = new ImmunizationCdcCatalogs
                {
                    CvxCatalogUID = "CvxCatalogUID",
                    VaccineGroupCatalogUID = "VaccineGroupCatalogUID"
                };

                _cacheClientMock
                    .Setup(x => x.GetAsync<ImmunizationCdcCatalogs>(It.IsAny<string>()))
                    .Returns(Task.FromResult(immunizationCdcCatalogs));
            }
            else
            {
                _cacheClientMock
                    .Setup(x => x.AddAsync(
                        It.IsAny<string>(),
                        It.IsAny<ImmunizationCdcCatalogs>(),
                        It.IsAny<DateTimeOffset?>()))
                    .Returns(Task.FromResult(true));

                _cacheClientMock
                    .Setup(x => x.GetAsync<ImmunizationCdcCatalogs>(It.IsAny<string>()))
                    .Returns(Task.FromResult(default(ImmunizationCdcCatalogs)));
            }

            var cdcContentModelCatalogs = default(CdcContentModelCatalog[]);

            if (!isEmptyCatalogs)
            {
                cdcContentModelCatalogs = new CdcContentModelCatalog[]
                {
                    new CdcContentModelCatalog
                    {
                        Mnemonic = "CDC_IMMUN_CVX",
                        CatalogUID = "CvxCatalogUID"
                    },
                    new CdcContentModelCatalog
                    {
                        Mnemonic = "CDC_IMMUN_VAC_GRP",
                        CatalogUID = "VaccineGroupCatalogUID"
                    }
                };
            }

            _cdcApiClientMock
                .Setup(x => x.GetContentModelCatalogsAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(cdcContentModelCatalogs));

            return immunizationCdcCatalogs;
        }

        private Dictionary<string, string> BuildCvxCodeVaccineGroupNameDictionary(string[] cvxCodes)
        {
            var cvxCodeVaccineGroupNameDictionary = new Dictionary<string, string>();

            var uncachedCodes = new List<string>();
            var apiRelations = new List<CdcCatalogSourceCodeRelations>();

            for (int i = 0; i < cvxCodes?.Length; i++)
            {
                var cvxCode = cvxCodes[i];
                var vaccineGroupName = $"VaccineGroupNameFor{cvxCode}";
                if (i % 2 == 0)
                {
                    _cacheClientMock
                        .Setup(x => x.GetAsync<string>(_cacheKeyBuilder.GetCacheKey(CvxCodeAlias, cvxCode)))
                        .Returns(Task.FromResult(vaccineGroupName));
                }
                else
                {
                    uncachedCodes.Add(cvxCode);

                    apiRelations.Add(new CdcCatalogSourceCodeRelations
                    {
                        RequestedTerm = new CdcTermRequestItem
                        {
                            TermSourceCode = cvxCode
                        },
                        RelatedItems = new[]
                        {
                            new CdcRelatedTermItem
                            {
                                CatalogUID = "VaccineGroupCatalogUID",
                                RelationshipMnemonic = RelationshipMnemonic,
                                RelatedTerm = new CdcTermSearchItem
                                {
                                    TermDescription = i % 3 == 0 ? null : vaccineGroupName
                                }
                            }
                        }
                    });

                    _cacheClientMock
                        .Setup(x => x.GetAsync<string>(_cacheKeyBuilder.GetCacheKey(CvxCodeAlias, cvxCode)))
                        .Returns(Task.FromResult(default(string)));

                    _cacheClientMock
                        .Setup(x => x.AddAsync(
                            _cacheKeyBuilder.GetCacheKey(CvxCodeAlias, cvxCode),
                            It.IsAny<string>(),
                            It.IsAny<DateTimeOffset?>()))
                        .Returns(Task.FromResult(true));
                }
            }

            _cdcApiClientMock
                .Setup(x => x.FindContentModelCatalogRelationsByCodesAsync(
                    ContentModel,
                    "CvxCatalogUID",
                    It.IsAny<string[]>()))
                .Returns(Task.FromResult(apiRelations.ToArray()));

            return cvxCodeVaccineGroupNameDictionary;
        }
    }
}
