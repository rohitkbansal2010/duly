// <copyright file="SystemContractsToRepositoryModelsProfile.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Client;
using NgdpApi = Duly.Ngdp.Api.Client;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Mappings
{
    /// <summary>
    /// Holds mappings for conversion from system level contracts to repository models.
    /// </summary>
    public class SystemContractsToRepositoryModelsProfile : Profile
    {
        public SystemContractsToRepositoryModelsProfile()
        {
            // Custom types.
            CreateMap<PatientPhoto, Models.PatientPhoto>();
            CreateMap<Attachment, Models.Attachment>();
            CreateMap<HumanName, Models.HumanName>();
            CreateMap<Role, Models.Role>();
            CreateMap<TimeSlot, Models.TimeSlot>();
            CreateMap<Location, Models.Location>();
            CreateMap<MemberType, Models.MemberType>();
            CreateMap<MemberRole, Models.MemberRole>();
            CreateMap<Gender, Models.Gender>();
            CreateMap<Repeat, Models.Repeat>();

            CreateMap<Models.CareTeamStatus, CareTeamStatus>();
            CreateMap<Models.CareTeamCategory, CareTeamCategory>();

            CreateMap<Period, Models.Period>();
            CreateMap<Quantity, Models.Quantity>();
            CreateMap<Drug, Models.Drug>();
            CreateMap<MedicationReason, Models.MedicationReason>();
            CreateMap<MedicationStatus, Models.MedicationStatus>();
            CreateMap<MedicationCategory, Models.MedicationCategory>();
            CreateMap<DaysOfWeek, Models.DaysOfWeek>();
            CreateMap<EventTiming, Models.EventTiming>();
            CreateMap<UnitsOfTime, Models.UnitsOfTime>();

            // With nested custom types.
            CreateMap<HumanGeneralInfo, Models.Abstractions.HumanGeneralInfo>();
            CreateMap<HumanGeneralInfoWithPhoto, Models.HumanGeneralInfoWithPhoto>().IncludeAllDerived();
            CreateMap<PractitionerGeneralInfo, Models.PractitionerGeneralInfo>();
            CreateMap<Participant, Models.Participant>();
            CreateMap<PatientGeneralInfo, Models.PatientGeneralInfo>();
            CreateMap<PatientGeneralInfoWithVisitsHistory, Models.PatientGeneralInfoWithVisitsHistory>();
            CreateMap<Patient, Models.Patient>()
                .ForMember(hn => hn.PatientAddress, opt => opt.Ignore())
                .ForMember(hn => hn.PhoneNumber, opt => opt.Ignore());

            CreateMap<Orders, Models.Orders>();

            CreateMap<NgdpApi.Appointment, Models.Appointment>();
            CreateMap<NgdpApi.AppointmentVisit, Models.AppointmentVisit>();
            CreateMap<NgdpApi.TimeSlot, Models.TimeSlot>();

            CreateMap<NgdpApi.HumanName, Models.HumanName>()
                .ForMember(hn => hn.Prefixes, opt => opt.Ignore())
                .ForMember(hn => hn.Suffixes, opt => opt.Ignore())
                .ForMember(hn => hn.Use, opt => opt.Ignore());

            CreateMap<NgdpApi.Patient, Models.AppointmentPatient>();
            CreateMap<NgdpApi.Practitioner, Models.AppointmentPractitioner>();

            CreateMap<CareTeamParticipant, Models.CareTeamParticipant>();
            CreateMap<CareTeamMember, Models.CareTeamMember>().IncludeAllDerived();
            CreateMap<PractitionerInCareTeam, Models.PractitionerInCareTeam>();

            CreateMap<Repeat, Models.Repeat>();
            CreateMap<Timing, Models.Timing>();
            CreateMap<Dosage, Models.MedicationDosage>();
            CreateMap<Medication, Models.Medication>();

            CreateMap<Observation, Models.Observation>();
            CreateMap<ObservationType, Models.ObservationType>().ReverseMap();
            CreateMap<ObservationComponent, Models.ObservationComponent>();
            CreateMap<ObservationComponentType, Models.ObservationComponentType>();
            CreateMap<ObservationMeasurement, Models.ObservationMeasurement>();

            CreateMap<AllergyIntolerance, Models.AllergyIntolerance>();
            CreateMap<AllergyIntoleranceReaction, Models.AllergyIntoleranceReaction>();

            CreateMap<Condition, Models.Condition>();

            CreateMap<DiagnosticReport, Models.DiagnosticReport>();
            CreateMap<ObservationLabResult, Models.ObservationLabResult>();
            CreateMap<ObservationLabResultComponent, Models.ObservationLabResultComponent>();
            CreateMap<ObservationLabResultInterpretation, Models.ObservationLabResultInterpretation>();
            CreateMap<ObservationLabResultMeasurement, Models.ObservationLabResultMeasurement>();
            CreateMap<ObservationLabResultReferenceRange, Models.ObservationLabResultReferenceRange>();

            CreateMap<ImmunizationStatus, Models.PastImmunizationStatus>().ReverseMap();
            CreateMap<NgdpApi.DueStatus, Models.RecommendedImmunizationStatus>().ReverseMap();

            CreateMap<ImmunizationStatusReason, Models.PastImmunizationStatusReason>();
            CreateMap<Vaccine, Models.PastImmunizationVaccine>();
            CreateMap<NgdpApi.Patient, Models.RecommendedImmunizationPatient>();
            CreateMap<NgdpApi.Immunization, Models.RecommendedImmunization>();
            CreateMap<Immunization, Models.PastImmunization>();
            CreateMap<Models.CheckOut.LabDetails, NgdpApi.LabDetails>();

            CreateMap<NgdpApi.ProviderLocation, Models.Provider>();
            CreateMap<NgdpApi.LabLocation, Models.LabLocation>();

            CreateMap<NgdpApi.ProviderDetails, Models.CheckOut.ProviderDetails>();
            CreateMap<NgdpApi.Pharmacy, Models.Pharmacy>();
            CreateMap<NgdpApi.DepartmentVisitType, Models.AppointmentByCsnId>()
                .ConvertUsing<AppointmentByCsnIdConverter>();
            CreateMap<Slot, Models.CheckOut.Slot>();
            CreateMap<ScheduleDay, Models.CheckOut.ScheduleDate>()
                .ConvertUsing<ScheduleDateConverter>();
            CreateMap<ScheduledAppointment, Models.CheckOut.ScheduledAppointment>()
                .ConvertUsing<ScheduleDateConverter>();
            CreateMap<Models.CheckOut.ScheduleAppointmentModel, ScheduleAppointmentModel>();

            CreateMap<PatientAddress, Models.PatientAddress>();
            CreateMap<PhoneNumber, Models.PhoneNumber>();

            CreateMap<Models.CarePlan.GetPatientLifeGoalByPatientPlanIdModel, Contracts.CarePlan.GetPatientLifeGoalByPatientPlanId>();

            CreateMap<Models.CarePlan.GetPatientConditionByPatientPlanIdModel, Contracts.CarePlan.GetPatientConditionByPatientPlanId>();

            CreateMap<Models.PatientPhotoByParam, PatientPhotoByParam>();

            CreateMap<NgdpApi.PatientAndAppointmentCount, Models.Dashboard.PatientAndAppointmentCount>();
        }
    }
}
