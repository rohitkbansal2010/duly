CREATE TABLE [dulycv].[ReferralAppointment] (
    [ReferralId]          VARCHAR (100)      NOT NULL,
    [ApptCSN]             VARCHAR (100)      NULL,
    [ApptDate]            DATE               NULL,
    [ApptTime]            TIME (7)           NULL,
    [ApptTimeZone]        VARCHAR (100)      NULL,
    [ApptDurationInMins]  INT                NULL,
    [ProviderDisplayName] VARCHAR (100)      NULL,
    [ProviderExternalId]  VARCHAR (100)      NULL,
    [ProviderPhotoURL]    VARCHAR (100)      NULL,
    [VisitTypeExternalId] VARCHAR (100)      NULL,
    [DeptExternalId]      VARCHAR (100)      NULL,
    [DeptName]            VARCHAR (100)      NULL,
    [DeptAddrSt]          VARCHAR (100)      NULL,
    [DeptCity]            VARCHAR (100)      NULL,
    [DeptState]           VARCHAR (100)      NULL,
    [DeptZip]             VARCHAR (100)      NULL,
    [ApptScheduledTime]   DATETIMEOFFSET (7) CONSTRAINT [DF_dulycv_ReferralAppointment_ApptScheduledTime] DEFAULT (getutcdate()) NOT NULL
);

