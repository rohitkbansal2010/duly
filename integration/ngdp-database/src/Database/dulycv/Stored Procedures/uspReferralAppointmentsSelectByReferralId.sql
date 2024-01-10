-- =============================================
-- Description: Select refferal appointments data from corresponfing table
-- =============================================

CREATE PROCEDURE [dulycv].[uspReferralAppointmentsSelectByReferralId]
    @ReferralId             VARCHAR(100)

AS
BEGIN
    SELECT [ReferralId]
      ,[ApptCSN] as AppointmentCSN
      ,[ApptDate] as AppointmentDate
      ,[ApptTime] as AppointmentTime
      ,[ApptTimeZone] as AppointmentTimeZone
      ,[ApptDurationInMins] as AppointmentDurationInMins
      ,[ProviderDisplayName]
      ,[ProviderExternalId]
      ,[ProviderPhotoURL]
      ,[VisitTypeExternalId]
      ,[DeptExternalId] as DepartmentExternalId
      ,[DeptName] as DepartmentName
      ,[DeptAddrSt] as DepartmentStreet
      ,[DeptCity] as DepartmentCity
      ,[DeptState] as DepartmentState
      ,[DeptZip] as DepartmentZip
      ,[ApptScheduledTime] as AppointmentScheduledTime
  FROM [dulycv].[ReferralAppointment]
  WHERE [ReferralId] = @ReferralId
END
