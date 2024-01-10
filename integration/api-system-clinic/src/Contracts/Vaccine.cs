// <copyright file="Vaccine.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Record of vaccine")]
    public class Vaccine
    {
        [Description("CVX codes of the vaccine substance administered.")]
        public string[] CvxCodes { get; set; }

        [Description("Name of vaccine product administered")]
        [Required]
        public string Text { get; set; }
    }
}
