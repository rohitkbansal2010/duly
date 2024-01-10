// <copyright file="Department.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("A grouping of people or organizations with a common purpose")]
    public class Department
    {
        [Description("Identifier of the Department")]
        [Required]
        public Identifier Identifier { get; set; }

        [Description("Name used for the organization")]
        public string Name { get; set; }

        [Description("Contact details (telephone, email, etc.) for a contact")]
        public ContactPoint[] ContactPoints { get; set; }
    }
}