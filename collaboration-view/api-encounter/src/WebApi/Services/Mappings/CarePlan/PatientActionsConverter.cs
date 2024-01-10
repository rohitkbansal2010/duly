// <copyright file="PatientActionsConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    public class PatientActionsConverter : ITypeConverter<Contracts.CarePlan.PatientActions, Models.PatientActions>
    {
        public PatientActions Convert(Contracts.CarePlan.PatientActions source, PatientActions destination, ResolutionContext context)
        {
            return new Models.PatientActions
            {
                PatientActionId = source.PatientActionId,
                ActionId = source.ActionId,
                CustomActionId = source.CustomActionId,
                PatientTargetId = source.PatientTargetId,
                Deleted = source.Deleted
            };
        }
    }
}