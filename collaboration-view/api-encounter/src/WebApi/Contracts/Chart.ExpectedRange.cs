// -----------------------------------------------------------------------
// <copyright file="Chart.ExpectedRange.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]
    [Description("Expected range for data")]
    public class ExpectedRange<T>
    {
        public ExpectedRange()
        {
            InclusiveLowerBound = true;
            InclusiveUpperBound = true;
        }

        [Description("Inclusive Lower Bound. By default is true")]
        [Required]
        public bool InclusiveLowerBound { get; set; }

        [Description("Inclusive Upper Bound. By default is true")]
        [Required]
        public bool InclusiveUpperBound { get; set; }

        [Description("Minimum")]
        public T Min { get; set; }

        [Description("Maximum")]
        public T Max { get; set; }
    }
}