-- =============================================
-- Description: Gets recommended providers for certain referralId
-- @ReferralId Id of referral which is provided to choose recommended providers
-- =============================================

CREATE PROCEDURE [dulycv].[uspReferralRecommendedProvidersSelect]
	@ReferralId VARCHAR(100)
AS
BEGIN

DECLARE @IdType VARCHAR (100) = 'External';

SELECT [Identifier] as Identifier
		,[referral_id] as ReferralId
		,@IdType as ReferralIdType
		,[referral_status] as ReferralStatus
		,[pat_id] as PatientId
		,[pat_last_name] as PatientLastName
		,[pat_first_name] as PatientFirstName
		,CONVERT(date, [pat_dob]) as PatientDateOfBirth
		,[pat_addr_1] as PatientAddressFirstLine
		,[pat_addr_2] as PatientAddressSecondLine
		,[pat_city] as PatientCity
		,[pat_state] as PatientState
		,[pat_zip] as PatientZip
		,[pat_phone] as PatientPhone
		,[specialty_id] as SpecialtyId
		,@IdType as SpecialtyIdType
		,[specialty_name] as SpecialtyName
		,[referredto_provider_id] as ReferredToProviderId
		,@IdType as ReferredToProviderIdType
		,[referredto_provider_name] as ReferredToProviderName
		,[department_id] as DepartmentId
		,@IdType as DepartmentIdType
		,[department_name] as DepartmentName
		,[department_phone] as DepartmentPhone
		,[location_id] as LocationId
		,@IdType as LocationIdType
		,[location_name] as LocationName
		,[location_addr_1] as LocationAddressFirstLine
		,[location_addr_2] as LocationAddressSecondLine
		,[location_city] as LocationCity
		,[location_state] as LocationState
		,[location_zip] as LocationZip
		,[location_distance_from_patient_home] as LocationDistanceFromPatientHome
		,[visit_type_id] as VisitTypeId
		,@IdType as VisitTypeIdType
		,[visit_type_name] as VisitTypeName
		,[provider_id] as ProviderId
		,@IdType as ProviderIdType
		,[provider_specialty] as ProviderSpecialty
		,[provider_name] as ProviderName
		,[slot_available_in_next_2_weeks] as IsSlotAvailableInNextTwoWeeks
  FROM [dulycv].[ReferralRecommendedProviders]
  WHERE [referral_id] = @ReferralId
END