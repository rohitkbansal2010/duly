// <copyright file="TimeZoneConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Api.Configurations;
using Microsoft.Extensions.Options;
using System;

namespace Duly.Ngdp.Api.Repositories.Mappings.Converters
{
    /// <inheritdoc cref="ITimeZoneConverter"/>
    public class TimeZoneConverter : ITimeZoneConverter
    {
        private readonly IOptionsMonitor<NgdpTimeZone> _timeZoneSettingsMonitor;

        public TimeZoneConverter(IOptionsMonitor<NgdpTimeZone> timeZoneSettingsMonitor)
        {
            _timeZoneSettingsMonitor = timeZoneSettingsMonitor;
        }

        public DateTimeOffset ToCstDateTimeOffset(DateTime dateTime)
        {
            return new DateTimeOffset(dateTime, GetTimeZoneInfo().GetUtcOffset(dateTime));
        }

        public DateTime ToCstDateTime(DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToOffset(GetTimeZoneInfo().GetUtcOffset(dateTimeOffset)).DateTime;
        }

        private TimeZoneInfo GetTimeZoneInfo() =>
            TimeZoneInfo.FindSystemTimeZoneById(_timeZoneSettingsMonitor.CurrentValue.DefaultName);
    }
}