// <copyright file="SitesConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    public class SitesConverter : ITypeConverter<Models.Site, Site>
    {
        public Site Convert(Models.Site source, Site destination, ResolutionContext context)
        {
            return new Site
            {
                Id = source.Id,
                Address = BulidAddress(source)
            };
        }

        private static Address BulidAddress(Models.Site source)
        {
            Address address = new();
            address.Line = source.Line;
            address.City = source.City;
            address.State = source.State;
            address.PostalCode = source.PostalCode;

            return address;
        }
    }
}
