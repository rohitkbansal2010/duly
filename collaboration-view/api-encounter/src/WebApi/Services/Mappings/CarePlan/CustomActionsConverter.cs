// <copyright file="CustomActionsConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings.CarePlan
{
    public class CustomActionsConverter : ITypeConverter<Contracts.CarePlan.CustomActions, Models.CustomActions>
    {
        public Models.CustomActions Convert(Contracts.CarePlan.CustomActions source, Models.CustomActions destination, ResolutionContext context)
        {
            return new Models.CustomActions
            {
                PatientTargetId = source.PatientTargetId,
                ActionName = source.ActionName,
                Description = source.Description,
                IsSelected = source.IsSelected
            };
        }
    }
}
