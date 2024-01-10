-- =============================================
-- Description: Gets immunizations for specific patient
-- @PatientId patient identifier
-- @IncludedDueStatusIds comma-separated string with Ids of the statuses.
-- =============================================

CREATE PROCEDURE [dulycv].[uspImmunizationsSelectByPatientId]
	@PatientId varchar(18),
    @IncludedDueStatusIds VARCHAR(MAX)
AS
BEGIN
  DECLARE @dueStatusIds TABLE ([Value] INT);

  INSERT INTO @dueStatusIds([Value])
  SELECT UPPER(TRIM(Value)) FROM STRING_SPLIT(@IncludedDueStatusIds, ',');

  SELECT
       [pat_id] AS PatientExternalId
      ,[vaccine_name] AS VaccineName
      ,[ideal_return_dt] AS DueDate
      ,[hmt_due_status_c] AS StatusId

  FROM [dulycv].[hm_immunizations]
  WHERE
  [pat_id] = @PatientId
  AND hmt_due_status_c IN (SELECT [Value] FROM @dueStatusIds)
END