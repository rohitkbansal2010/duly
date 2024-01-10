// <copyright file="NgdpToSystemApiContractsProfile.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Api.Repositories.Mappings;
using Duly.Ngdp.Contracts;
using System.Collections.Generic;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Configurations
{
    public class NgdpToSystemApiContractsProfile : Profile
    {
        public NgdpToSystemApiContractsProfile()
        {
            CreateMap<AppointmentStatusParam, AdapterModels.AppointmentStatus>();

            CreateMap<AdapterModels.Appointment, Appointment>()
                .ConvertUsing<AppointmentConverter>();

            CreateMap<AdapterModels.Immunization, Immunization>()
                .ConvertUsing<ImmunizationConverter>();

            CreateMap<AdapterModels.RecommendedProvider, RecommendedProvider>()
                .ConvertUsing<RecommendedProviderConverter>();

            CreateMap<AdapterModels.RecommendedProvider, ReferralPatient>()
                .ConvertUsing<RecommendedProviderConverter>();

            CreateMap<AdapterModels.ProviderLocation, ProviderLocation>()
                .ConvertUsing<ProviderLocationConverter>();

            CreateMap<AdapterModels.LabLocation, LabLocation>()
               .ConvertUsing<LabLocationConverter>();

            CreateMap<AdapterModels.ReferralAppointment, ReferralAppointment>()
                .ConvertUsing<ReferralAppointmentConverter>();

            CreateMap<LabDetails, AdapterModels.LabDetails>()
                .ConvertUsing<LabDetailsConverter>();

            CreateMap<AdapterModels.ProviderDetails, ProviderDetails>()
                .ConvertUsing<GetProviderDetailsConverter>();

            CreateMap<AdapterModels.Pharmacy, Pharmacy>()
                .ConvertUsing<GetPharmacyDetailConverter>();

            CreateMap<AdapterModels.DepartmentVisitType, DepartmentVisitType>()
                .ConvertUsing<GetSlotDataConverter>();
            CreateMap<AdapterModels.Dashboard.PatientAndAppointmentCount, Contracts.Dashboard.PatientAndAppointmentCount>();
        }
    }
}