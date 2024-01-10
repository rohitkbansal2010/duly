// <copyright file="Location.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Represents an information about a location")]
    public class Location : IDulyResource
    {
        [Description("Title of the location. It could be a number, or another human readable name of the location")]
        [Required]
        public string Title { get; set; }
    }
}
