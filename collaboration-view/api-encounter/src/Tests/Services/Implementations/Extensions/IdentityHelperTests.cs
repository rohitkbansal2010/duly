// <copyright file="IdentityHelperTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Services.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations.Extensions
{
    [TestFixture]
    public class IdentityHelperTests
    {
        public const string ExternalIdPrefix = "EXTERNAL|";

        [Test]
        public void ToIdWithExternalPrefix_Test()
        {
            var testId = "test-id";
            var testIdWithPrefix = $"{ExternalIdPrefix}test-id";

            var result = testId.ToIdWithExternalPrefix();

            result.Should().Be(testIdWithPrefix);
        }

        [Test]
        public void FindIdWithExternalPrefix_Test()
        {
            var testId = "test-id";
            var testIdWithPrefix = $"{ExternalIdPrefix}test-id";
            var ids = new[] { testId, testIdWithPrefix };

            var result = ids.FindIdWithExternalPrefix();

            result.Should().Be(testIdWithPrefix);
        }

        [Test]
        public void SplitIdWithExternalPrefix_Test()
        {
            var id = "test-id";
            var idWithPrefix = $"{ExternalIdPrefix}test-id";

            var result = idWithPrefix.SplitIdWithExternalPrefix();

            result.Should().Be(id);
        }
    }
}
