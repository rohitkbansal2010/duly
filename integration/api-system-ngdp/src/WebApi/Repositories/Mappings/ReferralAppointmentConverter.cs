// <copyright file="ReferralAppointmentConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Contracts;
using System;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps <see cref="AdapterModels.ReferralAppointment"/> into <see cref="ReferralAppointment"/>.
    /// </summary>
    public class ReferralAppointmentConverter : ITypeConverter<AdapterModels.ReferralAppointment, ReferralAppointment>
    {
        public ReferralAppointment Convert(AdapterModels.ReferralAppointment source, ReferralAppointment destination, ResolutionContext context)
        {
            return new ReferralAppointment
            {
                ReferralId = source.ReferralId,
                Visit = BuildVisit(source),
                ScheduledTime = BuildScheduledTime(source),
                Provider = BuildProvider(source),
                Department = BuildDepartment(source),
                Location = BuildLocation(source),
                Appointment = BuildAppointment(source)
            };
        }

        private static ScheduledAppointment BuildAppointment(AdapterModels.ReferralAppointment source)
        {
            return new ScheduledAppointment
            {
                Id = source.AppointmentCSN,
                DateTime = BuildScheduledDateTime(source),
                DurationInMinutes = source.AppointmentDurationInMins.GetValueOrDefault(),
                TimeZone = source.AppointmentTimeZone
            };
        }

        private static Location BuildLocation(AdapterModels.ReferralAppointment source)
        {
            return new Location
            {
                Address = new Address
                {
                    City = source.DepartmentCity,
                    State = source.DepartmentState,
                    PostalCode = source.DepartmentZip,
                    Lines = new[] { source.DepartmentStreet }
                }
            };
        }

        private static Department BuildDepartment(AdapterModels.ReferralAppointment source)
        {
            return new Department
            {
                Identifier = new Identifier
                {
                    Type = IdentifierType.External,
                    Id = source.DepartmentExternalId
                },
                Name = source.DepartmentName
            };
        }

        private static ScheduledProvider BuildProvider(AdapterModels.ReferralAppointment source)
        {
            return new ScheduledProvider
            {
                Identifier = new Identifier
                {
                    Type = IdentifierType.External,
                    Id = source.ProviderExternalId
                },
                Name = source.ProviderDisplayName,
                PhotoURL = source.ProviderPhotoURL
            };
        }

        private static Visit BuildVisit(AdapterModels.ReferralAppointment source)
        {
            return new Visit
            {
                Type = new VisitType
                {
                    Identifier = new Identifier
                    {
                        Type = IdentifierType.External,
                        Id = source.VisitTypeExternalId
                    }
                }
            };
        }

        private static DateTime BuildScheduledTime(AdapterModels.ReferralAppointment source)
        {
            return source.AppointmentScheduledTime.GetValueOrDefault(DateTime.UtcNow).DateTime;
        }

        private static DateTime BuildScheduledDateTime(AdapterModels.ReferralAppointment source)
        {
            var date = source.AppointmentDate.GetValueOrDefault().Date;
            var time = source.AppointmentTime.GetValueOrDefault();
            return date.Add(time);
        }
    }
}