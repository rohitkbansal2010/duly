// <copyright file="Location.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Represents an information about a location")]
    public class Location
    {
        [Description("Title of the location. It could be a number, or another human readable name of the location")]
        [Required]
        public string Title { get; set; }
    }
}