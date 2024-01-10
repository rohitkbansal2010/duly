// -----------------------------------------------------------------------
// <copyright file="VitalsCard.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Represents information about the latest measurement of the vitals including in the card.")]
    public class VitalsCard
    {
        [Description("Type of the vitals cards.")]
        [Required]
        public VitalsCardType CardType { get; set; }

        [Description("Information about the latest measurement of the each included vitals.")]
        [Required]
        public Vital[] Vitals { get; set; }
    }
}
