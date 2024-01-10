// <copyright file="AppointmentConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Api.Repositories.Mappings.Converters;
using Duly.Ngdp.Contracts;
using System;
using System.Linq;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps <see cref="AdapterModels.Appointment"/> into <see cref="Appointment"/>.
    /// </summary>
    public class AppointmentConverter : ITypeConverter<AdapterModels.Appointment, Appointment>
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public AppointmentConverter(ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public Appointment Convert(AdapterModels.Appointment source, Appointment destination, ResolutionContext context)
        {
            return new Appointment
            {
                Id = source.CsnId.ToString(),
                Visit = BuildVisit(source),
                Patient = BuildPatient(source),
                Practitioner = BuildPractitioner(source),
                Status = BuildStatus(source),
                TimeSlot = GetTimeSlot(source.Time, source.Length),
                Note = source.Note,
                IsTelehealthVisit = source.IsTelehealthVisit.ConvertDbStringToBool(),
                IsProtectedByBtg = source.IsUnderBtg.ConvertDbStringToBool()
            };
        }

        private static AppointmentStatus BuildStatus(AdapterModels.Appointment source)
        {
            return source.StatusName switch
            {
                "Arrived" => AppointmentStatus.Arrived,
                "Canceled" => AppointmentStatus.Canceled,
                "Completed" => AppointmentStatus.Completed,
                "Left without seen" => AppointmentStatus.LeftWithoutSeen,
                "No Show" => AppointmentStatus.NoShow,
                "Scheduled" => AppointmentStatus.Scheduled,
                "Unresolved" => AppointmentStatus.Unresolved,
                "Charge Entered" => AppointmentStatus.ChargeEntered,
                _ => AppointmentStatus.Unknown
            };
        }

        private static Practitioner BuildPractitioner(AdapterModels.Appointment source)
        {
            return new()
            {
                Id = source.ProviderDphoneId,
                HumanName = GetHumanName(source.ProviderName)
            };
        }

        private static Patient BuildPatient(AdapterModels.Appointment source)
        {
            return new()
            {
                Id = source.PatientExternalId
            };
        }

        private static AppointmentVisit BuildVisit(AdapterModels.Appointment source)
        {
            return new AppointmentVisit
            {
                TypeDisplayName = string.IsNullOrWhiteSpace(source.VisitTypeDisplayName) ? null : source.VisitTypeDisplayName,
                Type = source.VisitType,
                TypeId = source.VisitTypeId
            };
        }

        private static HumanName GetHumanName(string name)
        {
            var nameParts = name.Split(',', StringSplitOptions.TrimEntries);
            var givenNames = nameParts.Skip(1).ToArray();
            return new()
            {
                FamilyName = nameParts[0],
                GivenNames = givenNames.Length == 0 ? null : givenNames
            };
        }

        private TimeSlot GetTimeSlot(DateTime startTime, int durationInMinutes)
        {
            var cstStartDateTime = _timeZoneConverter.ToCstDateTimeOffset(startTime);

            return new()
            {
                StartTime = cstStartDateTime,
                EndTime = cstStartDateTime.AddMinutes(durationInMinutes)
            };
        }
    }
}