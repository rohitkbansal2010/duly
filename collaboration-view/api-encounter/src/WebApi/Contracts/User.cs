// <copyright file="User.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Information about a user of health care application")]
    public class User
    {
        [Description("Id of the user")]
        [Required]
        public string Id { get; set; }

        [Description("Name of the user with parts and usage")]
        [Required]
        public HumanName Name { get; set; }

        [Description("Image of the user")]
        public Attachment Photo { get; set; }
    }
}
