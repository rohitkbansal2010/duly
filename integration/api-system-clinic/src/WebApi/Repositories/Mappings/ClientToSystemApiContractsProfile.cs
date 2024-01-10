// <copyright file="ClientToSystemApiContractsProfile.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;
extern alias stu3;
using AutoMapper;
using Duly.Clinic.Api.Repositories.Mappings.ObservationConverters;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Hl7.Fhir.Model;
using System;
using Period = Duly.Clinic.Contracts.Period;
using Quantity = Duly.Clinic.Contracts.Quantity;
using R4 = r4::Hl7.Fhir.Model;
using STU3 = stu3::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    /// <summary>
    /// Holds mappings for conversion from FHIR to API Contracts.
    /// </summary>
    public class ClientToSystemApiContractsProfile : Profile
    {
        public ClientToSystemApiContractsProfile()
        {
            //CreateMap<Fhir.Adapter.Contracts.PatientPhoto, Contracts.PatientPhoto>();
            CreateMap<Contracts.PatientPhotoByParam, Fhir.Adapter.Contracts.PatientPhotoByParam>();

            CreateMap<R4.AllergyIntolerance.AllergyIntoleranceCategory, AllergyIntoleranceCategory>();
            CreateMap<R4.AllergyIntolerance.AllergyIntoleranceSeverity, AllergyIntoleranceReactionSeverity>();
            CreateMap<R4.AllergyIntolerance.ReactionComponent, AllergyIntoleranceReaction>()
                .ConvertUsing<AllergyIntoleranceReactionConverter>();
            CreateMap<R4.AllergyIntolerance, AllergyIntolerance>()
                .ConvertUsing<AllergyIntoleranceConverter>();

            CreateMap<R4.HumanName.NameUse, NameUse>();

            CreateMap<R4.HumanName, HumanName>()
                .ConvertUsing<HumanNameConverter>();
            CreateMap<STU3.HumanName, HumanName>()
                .ConvertUsing<HumanNameConverter>();

            CreateMap<R4.Attachment, Attachment>()
                .ForMember(
                    dest => dest.Data,
                    opt =>
                        opt.MapFrom(src => src.Data == null ? null : System.Convert.ToBase64String(src.Data)));
            CreateMap<STU3.Attachment, Attachment>()
                .ForMember(
                    dest => dest.Data,
                    opt =>
                        opt.MapFrom(src => src.Data == null ? null : System.Convert.ToBase64String(src.Data)));

            CreateMap<PractitionerWithRoles, PractitionerGeneralInfo>()
                .ConvertUsing<PractitionerGeneralInfoConverter>();
            CreateMap<PractitionerWithRolesSTU3, PractitionerGeneralInfo>()
                .ConvertUsing<PractitionerGeneralInfoConverter>();

            CreateMap<R4.Encounter.EncounterStatus, EncounterStatus>()
                .ConvertUsing<EncounterStatusConverter>();
            CreateMap<Hl7.Fhir.Model.Period, TimeSlot>()
                .ConvertUsing<TimeSlotConverter>();
            CreateMap<R4.Encounter.LocationComponent, Location>()
                .ConvertUsing<LocationConverter>();

            CreateMap<R4.Patient, PatientGeneralInfo>()
                .ConvertUsing<PatientGeneralInfoConverter>();
            CreateMap<EncounterWithCompartments, Encounter>()
                .ConvertUsing<EncounterConverter>();

            CreateMap<RelatedPersonWithCompartments, RelatedPersonGeneralInfo>()
                .ConvertUsing<RelatedPersonGeneralInfoConverter>();

            CreateMap<R4.CareTeam, CareTeamGeneralInfo>();
            CreateMap<OrganizationWithCompartments, OrganizationGeneralInfo>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Resource.Id));

            CreateMap<PractitionerWithRoles, CareTeamParticipant>()
                .ConvertUsing<CareTeamParticipantConverter>();

            CreateMap<PatientWithCompartments, Patient>()
                .ConvertUsing<PatientConverter>();

            CreateMap<Hl7.Fhir.Model.Period, Period>()
                .ConvertUsing<PeriodConverter>();
            CreateMap<STU3.Timing.RepeatComponent, Repeat>()
                .ConvertUsing<RepeatConverter>();
            CreateMap<STU3.Timing, Timing>();
            CreateMap<Hl7.Fhir.Model.Quantity, Quantity>();
            CreateMap<STU3.Dosage, Dosage>()
                .ConvertUsing<DosageConverter>();
            CreateMap<MedicationStatementWithCompartments, Medication>()
                .ConvertUsing<MedicationConverter>();
            CreateMap<MedicationRequestWithCompartments, Medication>()
                .ConvertUsing<MedicationRequestConverter>();

            CreateMap<ObservationWithCompartments, Observation>()
                .ConvertUsing<ObservationConverter>();

            CreateMap<R4.Condition, Condition>()
                .ConvertUsing<ConditionConverter>();

            CreateMap<DiagnosticReportWithCompartments, DiagnosticReport>()
                .ConvertUsing<DiagnosticReportConverter>();

            CreateMap<R4.Observation, Observation>()
                .ConvertUsing<ObservationConverter>();

            CreateMap<R4.Observation, ObservationLabResult>()
                .ConvertUsing<ObservationConverter>();

            CreateMap<R4.Immunization, Immunization>()
                .ConvertUsing<ImmunizationConverter>();

            CreateMap<CodeableConcept, Vaccine>()
                .ConvertUsing<VaccineConverter>();

            CreateMap<R4.Immunization.ImmunizationStatusCodes, ImmunizationStatus>()
                .ConvertUsing<ImmunizationStatusConverter>();

            CreateMap<DataType, DateTimeOffset>()
                .ConvertUsing<DataTypeConverter>();

            CreateMap<Wipfli.Adapter.Client.Slot, Slot>();
            CreateMap<Wipfli.Adapter.Client.ScheduleDay, ScheduleDay>()
                .ConvertUsing<ScheduleDayConverter>();

            CreateMap<Wipfli.Adapter.Client.Provider, ScheduledProvider>().ForMember(dst => dst.Identifiers, expression => expression.MapFrom(src => src.IDs));
            CreateMap<Wipfli.Adapter.Client.Identity, string>().ConvertUsing(r => $"{r.Type.Trim()}|{r.ID.Trim()}");
            CreateMap<Wipfli.Adapter.Client.Appointment, ScheduledAppointment>().ForMember(dst => dst.ContactIds, expression => expression.MapFrom(src => src.ContactIDs));
            CreateMap<Wipfli.Adapter.Client.Department, ScheduledDepartment>().ForMember(dst => dst.Identifiers, expression => expression.MapFrom(src => src.IDs));
            CreateMap<Wipfli.Adapter.Client.Specialty, Specialty>();
            CreateMap<Wipfli.Adapter.Client.OfficialTimeZone, OfficialTimeZone>();
            CreateMap<Wipfli.Adapter.Client.Phone, Phone>();
            CreateMap<Wipfli.Adapter.Client.Address, Address>();
            CreateMap<Wipfli.Adapter.Client.State, State>();
            CreateMap<Wipfli.Adapter.Client.Country, Country>();
            CreateMap<Wipfli.Adapter.Client.District, District>();
            CreateMap<Wipfli.Adapter.Client.County, County>();
            CreateMap<Wipfli.Adapter.Client.Patient, ScheduledPatient>().ForMember(dst => dst.Identifiers, expression => expression.MapFrom(src => src.IDs));
            CreateMap<Wipfli.Adapter.Client.VisitType, ScheduledVisitType>().ForMember(dst => dst.Identifiers, expression => expression.MapFrom(src => src.IDs));
        }
    }
}