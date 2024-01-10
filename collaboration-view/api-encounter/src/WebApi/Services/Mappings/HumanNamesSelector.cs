// <copyright file="HumanNamesSelector.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Linq;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    public static class HumanNamesSelector
    {
        internal static Repositories.Models.HumanName SelectHumanNameByUse(Repositories.Models.HumanName[] names)
        {
            var name = names
                .OrderBy(name => name.Use)
                .FirstOrDefault(name => name.Use is Repositories.Models.NameUse.Usual or Repositories.Models.NameUse.Official);

            if (name == null)
                throw new ServiceNotMappedException("Could not find name with proper use");

            return name;
        }
    }
}