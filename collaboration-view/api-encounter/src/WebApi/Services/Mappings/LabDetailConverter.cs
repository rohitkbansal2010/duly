// <copyright file="LabDetailConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    public class LabDetailConverter : ITypeConverter<Contracts.LabDetail, Models.LabDetails>
    {
        public LabDetails Convert(LabDetail source, LabDetails destination, ResolutionContext context)
        {
            return new Models.LabDetails
            {
                ID = source.ID,
                Type = source.Type,
                Lab_ID = source.Lab_ID,
                Lab_Location = source.Lab_Location,
                Lab_Name = source.Lab_Name,
                CreatedDate = source.CreatedDate,
                Appointment_ID = source.Appointment_ID,
                Patient_ID = source.Patient_ID,
                Skipped = source.Skipped
            };
        }
    }
}
