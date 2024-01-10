// <copyright file="ITimeZoneConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Api.Repositories.Mappings.Converters
{
    /// <summary>
    /// Converter to handle CST time zone specific time conversions.
    /// </summary>
    public interface ITimeZoneConverter
    {
        /// <summary>
        /// Creates <see cref="DateTimeOffset"/> with proper offset for CST timezone.
        /// </summary>
        /// <param name="dateTime"><see cref="DateTime"/> in CST timezone.</param>
        /// <returns><see cref="DateTimeOffset"/> with proper CST timezone offset.</returns>
        DateTimeOffset ToCstDateTimeOffset(DateTime dateTime);

        /// <summary>
        /// Converts <see cref="DateTimeOffset"/> to <see cref="DateTime"/> in CST timezone.
        /// </summary>
        /// <param name="dateTimeOffset"><see cref="DateTimeOffset"/> to be converted to CST timezone.</param>
        /// <returns><see cref="DateTime"/> converted to CST timezone.</returns>
        DateTime ToCstDateTime(DateTimeOffset dateTimeOffset);
    }
}