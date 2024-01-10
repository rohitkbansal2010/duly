create table [dulycv].[ReferralAppointmentHistory]
(
    [AppointmentHistoryId] bigint identity(1, 1) not null,
    [ReferralId]           varchar(100)   not null,
    [Status]               varchar(150)   not null,
    [Metadata]             varchar(max)   null,
    [CreationTime]         datetimeoffset not null constraint DF_dulycv_ReferralAppointmentHistory_CreationTime default(getutcdate())

    constraint PK_dulycv_ReferralAppointmentHistory primary key clustered (AppointmentHistoryId asc) with (data_compression = page)
);
