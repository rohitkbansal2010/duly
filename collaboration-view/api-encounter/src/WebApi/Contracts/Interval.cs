// <copyright file="Interval.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    /// <inheritdoc />
    public class Interval : IValidatableObject
    {
        public Interval(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        /// <summary>
        /// The lower bound of the interval.
        /// </summary>
        public DateTime StartDate { get; }

        /// <summary>
        /// The upper bound of the interval.
        /// </summary>
        public DateTime EndDate { get; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate > EndDate)
                yield return new ValidationResult("The start date must be no greater than the end date.");
        }
    }
}