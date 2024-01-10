// <copyright file="RepositoryModelsToProcessContractsProfile.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Services.Mappings.CarePlan;
using Duly.Common.Security.Entities;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    /// <summary>
    /// Holds mappings for conversion from repository models to process level contracts.
    /// </summary>
    internal class RepositoryModelsToProcessContractsProfile : Profile
    {
        public RepositoryModelsToProcessContractsProfile()
        {
            CreateMap<Models.CarePlan.Condition, Contracts.CarePlan.Condition>();
            CreateMap<Models.CarePlan.ConditionTargets, Contracts.CarePlan.ConditionTargets>();
            CreateMap<Models.CarePlan.TargetActions, Contracts.CarePlan.TargetActions>();
            CreateMap<Models.CarePlan.PatientTargets, Contracts.CarePlan.PatientTargets>().ReverseMap();

            // contract to contract type.
            CreateMap<Contracts.PractitionerGeneralInfo, Contracts.Party>()
                .ConvertUsing<GeneralInfoConverter>();

            // Custom types.
            CreateMap<Models.PatientPhoto, Contracts.Attachment>().ConvertUsing<PatientPhotoConverter>();

            CreateMap<Models.Attachment, Contracts.Attachment>();
            CreateMap<Models.HumanName, Contracts.HumanName>();
            CreateMap<Models.TimeSlot, Contracts.TimeSlot>();
            CreateMap<Models.Location, Contracts.Location>();
            CreateMap<Models.Gender, Contracts.Gender>()
                .ConvertUsing<GenderConverter>();

            // With nested custom types.
            CreateMap<Models.PractitionerGeneralInfo, Contracts.PractitionerGeneralInfo>()
                .ConvertUsing<GeneralInfoConverter>();

            CreateMap<AzureActiveDirectoryUserAccountIdentity, Contracts.PractitionerGeneralInfo>()
                .ConvertUsing<AzureActiveDirectoryUserAccountIdentityConverter>();

            CreateMap<Models.PractitionerGeneralInfo, Contracts.Party>()
                .ConvertUsing<GeneralInfoConverter>();
            CreateMap<AzureActiveDirectoryUserAccountIdentity, Contracts.Party>()
                .ConvertUsing<AzureActiveDirectoryUserAccountIdentityConverter>();

            CreateMap<AzureActiveDirectoryUserAccountIdentity, Contracts.User>()
                .ConvertUsing<AzureActiveDirectoryUserAccountIdentityConverter>();

            CreateMap<Models.PatientGeneralInfo, Contracts.PatientGeneralInfo>()
                .ForMember(
                    dest => dest.HumanName,
                    opt => opt.MapFrom(src => HumanNamesSelector.SelectHumanNameByUse(src.Names)));
            CreateMap<Models.PatientGeneralInfoWithVisitsHistory, Contracts.PatientExtendedInfo>()
                .ForMember(
                    dest => dest.PatientGeneralInfo,
                    opt =>
                        opt.MapFrom(src => src.Patient))
                .ForMember(
                    dest => dest.IsNewPatient,
                    opt =>
                        opt.MapFrom(src => !src.HasPastVisits))
                ;
            CreateMap<Models.Patient, Contracts.Patient>()
                .ForMember(
                    dest => dest.GeneralInfo,
                    opt =>
                        opt.MapFrom(src => src.PatientGeneralInfo))
                .ForMember(
                    dest => dest.BirthDate,
                    opt =>
                        opt.MapFrom(src => src.BirthDate.Date))

                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .ForMember(
                    dest => dest.PatientAddress,
                    opt =>
                        opt.MapFrom(src => src.PatientAddress));

            CreateMap<Models.Participant, Contracts.Party>()
                .ConvertUsing<ParticipantToPartyConverter>();
            CreateMap<Models.CareTeamParticipant, Contracts.Party>()
                .ConvertUsing<CareTeamParticipantToPartyConverter>();
            CreateMap<Models.PractitionerInCareTeam, Contracts.Party>()
                .ConvertUsing<CareTeamParticipantToPartyConverter>();

            CreateMap<Models.Medication, Contracts.Medication>()
                .ConvertUsing<MedicationConverter>();

            CreateMap<Models.Observation, Contracts.VitalsCard>()
                .ConvertUsing<ObservationConverter>();
            CreateMap<Contracts.VitalsCardType, Models.ObservationType[]>()
                .ConvertUsing<ObservationTypeConverter>();
            CreateMap<Models.ObservationType, Contracts.VitalsCardType>()
                .ConvertUsing<VitalsCardTypeConverter>();
            CreateMap<Models.ObservationType, Contracts.VitalType>()
                .ConvertUsing<VitalTypeConverter>();
            CreateMap<Models.ObservationComponentType, Contracts.VitalMeasurementType>()
                .ConvertUsing<VitalMeasurementTypeConverter>();
            CreateMap<Models.ObservationType, Contracts.VitalMeasurementType>()
                .ConvertUsing<VitalMeasurementTypeConverter>();

            CreateMap<Models.AllergyIntolerance, Contracts.Allergy>()
                .ConvertUsing<AllergyConverter>();
            CreateMap<Models.AllergyIntoleranceReaction, Contracts.AllergyReaction>();

            CreateMap<Models.Condition, Contracts.HealthCondition>()
                .ConvertUsing<ConditionConverter>();

            CreateMap<Models.DiagnosticReport, Contracts.TestReport>()
                .ConvertUsing<TestReportConverter>();

            CreateMap<Models.DiagnosticReportStatus, Contracts.TestReportStatus>()
                .ConvertUsing<TestReportStatusConverter>();
            CreateMap<Models.ObservationLabResultMeasurement, Contracts.TestReportResultMeasurement>();
            CreateMap<Models.ObservationLabResultReferenceRange, Contracts.TestReportResultReferenceRange>();
            CreateMap<Models.ObservationLabResult, Contracts.TestReportResult>()
                .ConvertUsing<TestReportResultConverter>();
            CreateMap<Models.DiagnosticReport, Contracts.TestReportWithResults>()
                .ForMember(
                    dest => dest.Title,
                    opt =>
                        opt.MapFrom(src => src.Name))
                .ForMember(
                    dest => dest.EffectiveDate,
                    opt =>
                        opt.MapFrom(src => src.EffectiveDate ?? default))
                .ForMember(
                    dest => dest.Results,
                    opt =>
                        opt.MapFrom(src => src.Observations))
                ;

            CreateMap<Models.PastImmunization, Contracts.Vaccination>()
                .ConvertUsing<VaccinationConverter>();

            CreateMap<Models.RecommendedImmunization, Contracts.ImmunizationsRecommendedGroup>()
                .ConvertUsing<ImmunizationsRecommendedGroupConverter>();

            CreateMap<Models.AppointmentStatus, Contracts.PatientAppointmentStatus>()
                .ConvertUsing<PatientAppointmentStatusConverter>();

            CreateMap<Contracts.LabDetail, Models.CheckOut.LabDetails>()
               .ConvertUsing<LabDetailConverter>();

            CreateMap<Contracts.ImagingDetail, Models.CheckOut.ImagingDetails>()
              .ConvertUsing<ImagingDetailConverter>();

            CreateMap<Contracts.ScheduleFollowUp, Models.CheckOut.ScheduleFollowUp>()
            .ConvertUsing<ScheduleFollowUpConverter>();

            CreateMap<Contracts.ScheduleFollowUp, Models.CheckOut.ScheduleFollowUp>()
              .ConvertUsing<ReferralConverter>();

            CreateMap<Contracts.ScheduleReferral, Models.CheckOut.ScheduleReferral>()
              .ConvertUsing<ScheduleReferralConverter>();

            CreateMap<Models.CheckOut.GetLabOrImaging, Contracts.GetLabOrImaging>()
             .ConvertUsing<GetLabDetailConverter>();

            CreateMap<Models.CheckOut.ScheduleReferral, Contracts.ScheduleReferral>()
              .ConvertUsing<GetScheduleFollowUpConverter>();

            CreateMap<Models.CheckOut.ProviderDetails, Contracts.ProviderDetails>()
              .ConvertUsing<GetProviderDetailConverter>();

            CreateMap<Models.Pharmacy, Contracts.Pharmacy>()
                .ConvertUsing<PharmacyConverter>();

            CreateMap<Models.CheckOut.ScheduleDate, Contracts.ScheduleDate>()
                .ConvertUsing<ScheduleDateConverter>();

            CreateMap<Models.CheckOut.ScheduledAppointment, Contracts.ScheduleAppointmentResult>()
                .ConvertUsing<ScheduleSlotsConverter>();

            CreateMap<Models.Orders, Contracts.Orders>();

            CreateMap<Contracts.CarePlan.PatientPlan, Models.CarePlan.PatientPlan>()
                .ConvertUsing<PatientPlanConverter>();

            CreateMap<Contracts.CarePlan.PatientConditions, Models.CarePlan.PatientConditions>()
                .ConvertUsing<PatientConditionsConverter>();
            CreateMap<Models.CarePlan.PostPatientConditionsResponse, Contracts.CarePlan.PostPatientConditionsResponse>();

            CreateMap<Contracts.CarePlan.CustomActions, Models.CarePlan.CustomActions>()
             .ConvertUsing<CustomActionsConverter>();

            CreateMap<Contracts.CarePlan.PatientActions, Models.CarePlan.PatientActions>()
            .ConvertUsing<PatientActionsConverter>();

            CreateMap<Models.CheckOut.Slot, Contracts.TimeSlots>();
            CreateMap<Models.PatientAddress, Contracts.PatientAddress>();
            CreateMap<Contracts.AfterVisitPdf, Models.AfterVisitPdf>();
            CreateMap<Contracts.CarePlan.UpdatePatientTargetReviewStatus, Models.CarePlan.UpdatePatientTargetReviewStatus>();
            CreateMap<Contracts.CarePlan.PatientLifeGoalTargetMapping, Models.CarePlan.PatientLifeGoalTargetMapping>();

            CreateMap<Contracts.CarePlan.PostRequestForLifeGoals, Models.CarePlan.PostRequestForLifeGoals>()
                .ConvertUsing<LifeGoalDetailsConverter>();

            CreateMap<Models.CarePlan.GetPatientTargets, Contracts.CarePlan.GetPatientTargets>();

            CreateMap<Models.CarePlan.GetPatientActions, Contracts.CarePlan.GetPatientActions>();
            CreateMap<Models.CarePlan.GetPatientLifeGoalTargetMapping, Contracts.CarePlan.GetPatientLifeGoalTargetMapping>();

            CreateMap<Models.CarePlan.PatientLifeGoalResponse, Contracts.CarePlan.PatientLifeGoalResponse>();
            CreateMap<Models.CarePlan.GetPatientPlanByPatientIdModel, Contracts.CarePlan.GetPatientPlanByPatientId>();
            CreateMap<Models.CarePlan.PatientLifeGoal, Contracts.CarePlan.PatientLifeGoal>();
            CreateMap<Models.CarePlan.UpdateFlourishStatementRequest, Contracts.CarePlan.UpdateFlourishStatementRequest>();
            CreateMap<Models.PhoneNumber, Contracts.PhoneNumber>();
            CreateMap<Contracts.CarePlan.UpdateActionProgress, Models.CarePlan.UpdateActionProgress>();

            CreateMap<Models.CarePlan.PostOrUpdatePatientLifeGoalResponse, Contracts.CarePlan.PostOrUpdatePatientLifeGoalResponse>()
                .ForMember(hn => hn.StatusCode, opt => opt.Ignore())
                .ForMember(hn => hn.Message, opt => opt.Ignore());

            CreateMap<Contracts.CarePlan.UpdatePatientTargets, Models.CarePlan.UpdatePatientTargets>();

            CreateMap<Models.CarePlan.GetHealthPlanStats, Contracts.CarePlan.GetHealthPlanStats>();
            CreateMap<Models.CarePlan.GetPatientActionStats, Contracts.CarePlan.GetPatientActionStats>();

            CreateMap<Models.Site, Contracts.Site>().ConvertUsing<SitesConverter>();

            CreateMap<Models.CarePlan.PatientLifeGoalAndActionTracking, Contracts.CarePlan.PatientLifeGoalAndActionTracking>();

            CreateMap<Models.Dashboard.PatientAndAppointmentCount, Contracts.Dashboard.PatientAndAppointmentCount>();
        }
    }
}
