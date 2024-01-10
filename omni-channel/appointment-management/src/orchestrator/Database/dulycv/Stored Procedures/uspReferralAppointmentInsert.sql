-----------------------------------------------------------------------------------------
--  Revision History
--  Date              Author                   Description
--  03/10/2022        Georgii Sergeev          Initial version
--  03/10/2022        Syarhei Prudnikaw        Adjustment of code style and naming
--
--  Description: Inserts refferal appointments data into corresponfing table
-----------------------------------------------------------------------------------------

create procedure [dulycv].[uspReferralAppointmentInsert]
    @referralId             varchar(100),
    @apptCSN                varchar(100),
    @apptDate               date,
    @apptTime               time(7),
    @apptTimeZone           varchar(100),
    @apptDurationInMins     int,
    @providerDisplayName    varchar(100),
    @providerExternalId     varchar(100),
    @providerPhotoURL       varchar(100),
    @visitTypeExternalId    varchar(100),
    @deptExternalId         varchar(100),
    @deptName               varchar(100),
    @deptAddrSt             varchar(100),
    @deptCity               varchar(100),
    @deptState              varchar(100),
    @deptZip                varchar(100),
    @apptScheduledTime      datetimeoffset

as
begin
    set nocount on;

    insert into [dulycv].[ReferralAppointment]
        (ReferralId,
         ApptCSN,
         ApptDate,
         ApptTime,
         ApptTimeZone,
         ApptDurationInMins,
         ProviderDisplayName,
         ProviderExternalId,
         ProviderPhotoURL,
         VisitTypeExternalId,
         DeptExternalId,
         DeptName,
         DeptAddrSt,
         DeptCity,
         DeptState,
         DeptZip,
         ApptScheduledTime
        )
    values
        (@referralId,
         @apptCSN,
         @apptDate,
         @apptTime,
         @apptTimeZone,
         @apptDurationInMins,
         @providerDisplayName,
         @providerExternalId,
         @providerPhotoURL,
         @visitTypeExternalId,
         @deptExternalId,
         @deptName,
         @deptAddrSt,
         @deptCity,
         @deptState,
         @deptZip,
         @apptScheduledTime
        );

    set nocount off;
end;