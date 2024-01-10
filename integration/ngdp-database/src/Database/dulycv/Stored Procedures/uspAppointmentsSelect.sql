-- =============================================
-- Description: Gets appiontments from certain site with on certain time frame for included visit types and without excuded statuses 
-- @DepartmentId Id of department where appoinment takes place
-- @ApptTimeLowerBound Lower bound of interval in whitch apointemtns are requested
-- @ApptTimeUpperBound Upper bound of interval in whitch apointemtns are requested
-- @ExcludedStatuses Statuses with which appointemtns should be excluded from results
-- @IncludedVisitTypeIds Type Ids with which appointemtns should be included to results
-- =============================================

CREATE PROCEDURE [dulycv].[uspAppointmentsSelect]
	@ApptTimeLowerBound DateTime,
	@ApptTimeUpperBound DateTime,
	@ExcludedStatuses VARCHAR(MAX),
	@IncludedVisitTypeIds VARCHAR(MAX),
    @DepartmentId numeric(18, 0) = null,
    @LocationId numeric(18, 0) = null
AS
BEGIN

  DECLARE @apptStatusNames TABLE ([Value] VARCHAR(250));
  DECLARE @visitTypeIds TABLE ([Value] VARCHAR(20));

  INSERT INTO @apptStatusNames([Value])
  SELECT UPPER(TRIM(Value)) FROM STRING_SPLIT(@ExcludedStatuses, ',');
 
  INSERT INTO @visitTypeIds([Value])
  SELECT UPPER(TRIM(Value)) FROM STRING_SPLIT(@IncludedVisitTypeIds, ',');

  SELECT [csn_id] AS CsnId
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
  WHERE
  (@DepartmentId IS NULL OR [department_id] = @DepartmentId)
  AND (@LocationId IS NULL OR [location_id] = @LocationId)
   
  AND [appt_time] BETWEEN @ApptTimeLowerBound AND @ApptTimeUpperBound 
  AND UPPER([appt_status_name]) NOT IN (SELECT [Value] FROM @apptStatusNames) 
  AND [visit_type_id] IN (SELECT [Value] FROM @visitTypeIds)
END