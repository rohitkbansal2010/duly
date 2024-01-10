-----------------------------------------------------------------------------------------
--  Revision History
--  Date              Author                   Description
--  03/10/2022        Syarhei Prudnikaw        Initial version
--
--  Description: Inserts refferal appointments data into corresponfing table
-----------------------------------------------------------------------------------------

create procedure [dulycv].[uspUpdateStatusByReferralId]
    @referralId     varchar(100),
    @status         varchar(150),
    @meta           varchar(max) = null

as
begin
    set nocount on;

    insert into [dulycv].[ReferralAppointmentHistory]
        (ReferralId,
         [Status],
         [Metadata]
        )
    values
        (@referralId,
         @status,
         @meta
        );

    set nocount off;
end;
