-- =============================================
-- Description: Gets appointments on certain time frame with incuded statuses 
--              for the patient which has an appointment with specific CSN identifier
-- @ApptTimeLowerBound Lower bound of interval in which appointments are requested
-- @ApptTimeUpperBound Upper bound of interval in which appointments are requested
-- @IncludedStatuses Statuses with which appointments should be included in results
-- @CsnId CSN identifier
-- =============================================

CREATE PROCEDURE [dulycv].[uspAppointmentsSelectForPatientByCsnId]
	@ApptTimeLowerBound DATETIME,
	@ApptTimeUpperBound DATETIME,
	@IncludedStatuses VARCHAR(MAX),
	@CsnId NUMERIC(18, 0)
AS
BEGIN
    DECLARE @patientId VARCHAR (18);
	DECLARE @apptStatusNames TABLE ([Value] VARCHAR(250));

	SELECT @patientId = (SELECT TOP(1) [pat_id] FROM [dulycv].[Appointments] WHERE [csn_id] = @CsnId);

	INSERT INTO @apptStatusNames([Value])
	SELECT UPPER(TRIM(Value)) FROM STRING_SPLIT(@IncludedStatuses, ',');

	SELECT
		[csn_id] AS CsnId
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

	FROM
		[dulycv].[Appointments]
	WHERE
		([csn_id] = @CsnId)
		OR
		([pat_id] = @patientId
		AND [appt_time] BETWEEN @ApptTimeLowerBound AND @ApptTimeUpperBound
		AND UPPER([appt_status_name]) IN (SELECT [Value] FROM @apptStatusNames))
END