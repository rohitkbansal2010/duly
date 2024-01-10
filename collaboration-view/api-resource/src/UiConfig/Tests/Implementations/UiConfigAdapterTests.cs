// <copyright file="UiConfigAdapterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Dapper;
using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.UiConfig.Adapter.Contracts;
using Duly.UiConfig.Adapter.Implementations;
using Duly.UiConfig.Adapter.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace UiConfig.Adapter.Tests.Implementations
{
    [TestFixture]
    public class UiConfigAdapterTests
    {
        private const string FindConfigurationsStoredProcedureName = "[core].[uspConfigurationsSelect]";
        private const string ApplicationPartParameterName = "@AppPart";
        private const string SiteIdParameterName = "@SiteId";
        private const string PatientIdParameterName = "@PatientId";
        private const string TargetAreaTypeParameterName = "@TargetAreaType";

        private Mock<IDapperContext> _dapperContextMocked;

        [SetUp]
        public void SetUp()
        {
            ConfigureDapperContext();
        }

        [Test]
        public async Task GetConfigurationsAsync()
        {
            //Arrange
            var searchCriteria = new UiConfigurationSearchCriteria
            {
                ApplicationPart = ApplicationPart.CurrentAppointment,
                TargetAreaType = UiConfigurationTargetAreaType.Navigation,
                SiteId = "SiteId",
                PatientId = "PatientId"
            };

            IUiConfigAdapter uiConfigAdapter = new UiConfigAdapter(_dapperContextMocked.Object);

            //Act
            var results = await uiConfigAdapter.GetConfigurationsAsync(searchCriteria);

            //Assert
            _dapperContextMocked.Verify(
                x => x.ExecuteJsonResultAsync<UiConfigurationWithChildren>(
                    FindConfigurationsStoredProcedureName,
                    It.Is<DynamicParameters>(
                        parameters => parameters.Get<string>(ApplicationPartParameterName) == searchCriteria.ApplicationPart.ToString()
                                        && parameters.Get<string>(SiteIdParameterName) == searchCriteria.SiteId
                                        && parameters.Get<string>(PatientIdParameterName) == searchCriteria.PatientId
                                        && parameters.Get<string>(TargetAreaTypeParameterName) == searchCriteria.TargetAreaType.ToString()),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>()),
                Times.Once());

            results.Should().NotBeNullOrEmpty();
            results.Should().AllBeOfType<UiConfigurationWithChildren>();
        }

        private void ConfigureDapperContext()
        {
            _dapperContextMocked = new Mock<IDapperContext>();

            _dapperContextMocked
                .Setup(ctx => ctx
                    .ExecuteJsonResultAsync<UiConfigurationWithChildren>(
                        It.IsAny<string>(),
                        It.IsAny<DynamicParameters>(),
                        It.IsAny<IDbTransaction>(),
                        It.IsAny<int?>()))
                .Returns(Task.FromResult(new[] { new UiConfigurationWithChildren() }.AsEnumerable()));
        }
    }
}