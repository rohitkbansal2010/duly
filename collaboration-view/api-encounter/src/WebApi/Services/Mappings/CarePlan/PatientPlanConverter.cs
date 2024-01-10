// <copyright file="PatientPlanConverter.cs" company="Duly Health and Care">
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

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings.CarePlan
{
    public class PatientPlanConverter : ITypeConverter<Contracts.CarePlan.PatientPlan, Models.PatientPlan>
    {
        public PatientPlan Convert(Contracts.CarePlan.PatientPlan source, PatientPlan destination, ResolutionContext context)
        {
            return new Models.PatientPlan
            {
                PatientId = source.PatientId,
                ProviderId = source.ProviderId,
                FlourishingStatement = source.FlourishingStatement,
                AppointmentId = source.AppointmentId
            };
        }
    }
}