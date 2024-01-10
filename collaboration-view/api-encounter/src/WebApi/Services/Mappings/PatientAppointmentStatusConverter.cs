// <copyright file="PatientAppointmentStatusConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    internal class PatientAppointmentStatusConverter : ITypeConverter<Models.AppointmentStatus, PatientAppointmentStatus>
    {
        public PatientAppointmentStatus Convert(Models.AppointmentStatus source, PatientAppointmentStatus destination, ResolutionContext context)
        {
            return source switch
            {
                Models.AppointmentStatus.Completed => PatientAppointmentStatus.Completed,
                Models.AppointmentStatus.Canceled => PatientAppointmentStatus.Cancelled,
                Models.AppointmentStatus.NoShow => PatientAppointmentStatus.NoShow,
                Models.AppointmentStatus.Scheduled => PatientAppointmentStatus.Scheduled,
                _ => PatientAppointmentStatus.Unknown
            };
        }
    }
}