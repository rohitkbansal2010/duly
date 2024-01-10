create table [dulycv].[ReferralAppointment] (
    [ReferralId]          varchar(100)   not null,
    [ApptCSN]             varchar(100)   null,
    [ApptDate]            date           null,
    [ApptTime]            time(7)        null,
    [ApptTimeZone]        varchar(100)   null,
    [ApptDurationInMins]  int            null,
    [ProviderDisplayName] varchar(100)   null,
    [ProviderExternalId]  varchar(100)   null,
    [ProviderPhotoURL]    varchar(100)   null,
    [VisitTypeExternalId] varchar(100)   null,
    [DeptExternalId]      varchar(100)   null,
    [DeptName]            varchar(100)   null,
    [DeptAddrSt]          varchar(100)   null,
    [DeptCity]            varchar(100)   null,
    [DeptState]           varchar(100)   null,
    [DeptZip]             varchar(100)   null,
    [ApptScheduledTime]   datetimeoffset not null constraint DF_dulycv_ReferralAppointment_ApptScheduledTime default(getutcdate())
);

