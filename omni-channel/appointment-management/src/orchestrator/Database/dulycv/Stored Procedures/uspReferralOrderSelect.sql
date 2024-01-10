------------------------------------------------------------------------------------------------------
--  Revision History
--  Date              Author                   Description
--  03/24/2022        Sravan K. Golla          Initial version
--
--  Description:      Will fetch the list of referrals where system need to send SMS to patient.
--                    To avoid duplicate messages to patient, system will not fetch those referrals 
--				      where status of sent messages are 'INVITATION.SENT'
-----------------------------------------------------------------------------------------------------
create procedure [dulycv].[uspReferralOrderSelect]
as
begin
    set nocount on;

    select top 1 with ties 
           rrp.[referral_id] as [ReferralId], 
           rrp.[pat_id] as [PatientId], 
           rrp.[pat_phone] as [PatientPhone], 
           concat(rrp.[pat_first_name],' ',rrp.[pat_last_name]) as [PatientName], 
           rrp.[pat_dob] as [PatientDateOfBirth], 
           rrp.[specialty_id] as [SpecialtyId], 
           rrp.[specialty_name] as [Specialty], 
           rrp.[referredby_provider_id] as [ProviderExternalId], 
           rrp.[referredby_provider_name] as [ProviderName], 
           null as [ProviderPhotoURL]
    from [dulycv].[ReferralRecommendedProviders] rrp with (nolock) 
    left outer join [dulycv].[ReferralAppointmentHistory] rah with (nolock) 
         on rrp.[referral_id] = rah.ReferralId 
         and rah.[Status] = 'INVITATION.SENT'
    where rah.ReferralId is null
    order by row_number() over (partition by rrp.[referral_id] order by rrp.[referredby_provider_id] asc)  

	set nocount off;
end