-- =============================================
-- Description: Gets top 1 appointment by certain CsnId
-- @CsnId CSN identifier
-- =============================================

CREATE PROCEDURE [dulycv].[uspAppointmentSelectSingleByCsnId]
	@CsnId numeric(18, 0)
AS
BEGIN
SELECT TOP 1 [csn_id] AS CsnId
      ,[appt_note] AS Note
      ,[appt_length] AS Length
      ,[appt_status_name] AS StatusName
      ,[appt_time] AS Time

      ,[visit_type_id] AS VisitTypeId
      ,[visit_type] AS VisitType
      ,[visit_type_display_name] AS VisitTypeDisplayName

      ,[pat_id] AS PatientExternalId

      ,[visit_prov_id] AS ProviderDphoneId
      ,[NPI] AS ProviderNpiId
      ,[provider_name] AS ProviderName
      ,[telehealth_enc] AS IsTelehealthVisit
      ,[btg_flag] AS IsUnderBtg

  FROM [dulycv].[Appointments]
  Where 
  [csn_id] = @CsnId 
END