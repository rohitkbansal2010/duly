using Duly.UI.Flourish.GeneratePDF.Helpers;
using Duly.UI.Flourish.GeneratePDF.Model;
using PDFGeneratorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Duly.UI.Flourish.GeneratePDF.Views
{
    public static class ReferralsContainerView
    {
        public static string CreateRefferalsContainer(OpenReferralViewModel openReferrals, List<ScheduleReferral> scheduleReferral, AppointmentDetailsBySMSStatusViewModel pendingData)
        {
            if ((openReferrals == null || openReferrals.OpenRefferalResponse.Count() == 0) && (scheduleReferral.Count() <= 0 || scheduleReferral == null))
            {
                return "";
            }
                var body = "";
            body += @"<div class='page-break'><div class='header display--flex align--items--center location--flex'>
        <svg style='margin-right:0.5rem;' width='45' height='45' viewBox='0 0 60 60' fill='none' xmlns='http://www.w3.org/2000/svg'><circle cx='30' cy='30' r='30' fill='#00A5DF' fill-opacity='0.3'></circle><path d='M31.0003 15C29.0305 15 27.0799 15.388 25.26 16.1418C23.4402 16.8956 21.7866 18.0005 20.3937 19.3934C17.5806 22.2064 16.0003 26.0218 16.0003 30C15.9872 33.4637 17.1865 36.8228 19.3903 39.495L16.3903 42.495C16.1822 42.7059 16.0412 42.9738 15.9851 43.2648C15.929 43.5558 15.9604 43.8568 16.0753 44.13C16.1999 44.3999 16.4018 44.6267 16.6556 44.7816C16.9093 44.9365 17.2033 45.0125 17.5003 45H31.0003C34.9785 45 38.7938 43.4196 41.6069 40.6066C44.4199 37.7936 46.0003 33.9782 46.0003 30C46.0003 26.0218 44.4199 22.2064 41.6069 19.3934C38.7938 16.5804 34.9785 15 31.0003 15ZM31.0003 42H21.1153L22.5103 40.605C22.7897 40.324 22.9465 39.9438 22.9465 39.5475C22.9465 39.1512 22.7897 38.771 22.5103 38.49C20.5462 36.5281 19.323 33.9458 19.0493 31.1832C18.7756 28.4206 19.4682 25.6485 21.0091 23.3393C22.5501 21.0301 24.844 19.3265 27.5001 18.519C30.1561 17.7114 33.0101 17.8497 35.5756 18.9104C38.1411 19.971 40.2596 21.8884 41.5699 24.3358C42.8803 26.7833 43.3016 29.6093 42.762 32.3325C42.2224 35.0557 40.7552 37.5076 38.6106 39.2704C36.4659 41.0332 33.7764 41.9978 31.0003 42ZM35.5003 28.5H32.5003V25.5C32.5003 25.1022 32.3423 24.7206 32.0609 24.4393C31.7796 24.158 31.3981 24 31.0003 24C30.6025 24 30.2209 24.158 29.9396 24.4393C29.6583 24.7206 29.5003 25.1022 29.5003 25.5V28.5H26.5003C26.1025 28.5 25.7209 28.658 25.4396 28.9393C25.1583 29.2206 25.0003 29.6022 25.0003 30C25.0003 30.3978 25.1583 30.7794 25.4396 31.0607C25.7209 31.342 26.1025 31.5 26.5003 31.5H29.5003V34.5C29.5003 34.8978 29.6583 35.2794 29.9396 35.5607C30.2209 35.842 30.6025 36 31.0003 36C31.3981 36 31.7796 35.842 32.0609 35.5607C32.3423 35.2794 32.5003 34.8978 32.5003 34.5V31.5H35.5003C35.8981 31.5 36.2796 31.342 36.5609 31.0607C36.8423 30.7794 37.0003 30.3978 37.0003 30C37.0003 29.6022 36.8423 29.2206 36.5609 28.9393C36.2796 28.658 35.8981 28.5 35.5003 28.5Z' fill='#00A5DF'></path></svg>
        <span class='patient-detail display--flex font--weight--600 font--size--20'>Referrals</span>
        </div>";


            var labSectionStart = "";
            var referralsContainerStart = "";
            var container = "";
            if (openReferrals.OpenRefferalResponse != null)
            {
                var openRefs = new List<OpenReferralResponse>();
                foreach(var referral in openReferrals.OpenRefferalResponse)
                {
                    var flag = false;
                    foreach(var scheduledRef in scheduleReferral)
                    {
                        if(referral.Speciality == scheduledRef.RefVisitType)
                        {
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        openRefs.Add(referral);
                    }
                }
                openReferrals.OpenRefferalResponse = openRefs;
                referralsContainerStart = @"<div class='padded-white pb-16' style='padding-top: 16px; '>";
                referralsContainerStart += @" <div class='outer-div'> ";
                labSectionStart = @" <div class='pending-text'>Pending referrals for your appointment on {Appointment-Date} </div> ".Replace("{Appointment-Date}",pendingData.AppointmentTime);

                container += @"<div class='card-container'>";
                foreach (var referral in openReferrals.OpenRefferalResponse)
                {
                    container += @"
                        <div class='pending-card page-break'>
                            <div class='reffered-by-container'>
                                <div class='reffered-by-text'>
                                    REFERRED BY
                                </div>
                                <div class='reffered-by-provider'>
                                    
                                    <div class='reffered-by-name'>
                                        {Doctor-Name}
                                    </div>
                                </div>
                            </div>
                            <div class='reffered-to-container'>
                                <div class='reffered-by-text'>
                                    Referred to Specialty
                                </div>
                                <div class='reffered-to-speciality'>
                                    {Preferred-Speciality}
                                </div>
                            </div>
                        </div>".Replace("{Doctor-Name}", referral.ReferredByName).Replace("{Preferred-Speciality}", referral.Speciality);
                }
                labSectionStart += container + " </div> ";
                referralsContainerStart += labSectionStart + "</div>" + "</div>";
                if (openRefs.Count > 0)
                {
                    body += referralsContainerStart;
                }
            }

            if (scheduleReferral != null && scheduleReferral.Count() > 0)
            {
                referralsContainerStart = @"<div class='padded-white pb-16' style='padding-top: 16px; '>";
                referralsContainerStart += @" <div class='outer-div'> ";
                labSectionStart = @"  <div class='pending-text'>Scheduled </div> ";
                container = @"<div class='card-container'>";

                foreach(var referral in scheduleReferral)
                {
                    container += @"
<div class='scheduled-card page-break'>
                        <div class='scheduled-header'>{Schedule-Day}, {Schedule-Month} {Schedule-Date}, {Schedule-Year} - {Schedule-Time}</div>
                        <div class='scheduled-card-inner'>
                            <div class='reffered-by-container'>
                                <div class='reffered-by-text'>
                                    Scheduled With
                                </div>
                                <div class='reffered-by-provider scheduled'>
                                    <div>
                                        <div class='reffered-by-name'>
                                            {Doctor-Name}
                                        </div>
                                        <div class='reffered-by-speciality'>
                                            {Doctor-Speciality}
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class='reffered-to-container'>
                                <div class='reffered-by-text margin-bottom-4'>
                                    Location
                                </div>
                                <div class='reffered-to-location'>
                                    {Doctor-Location-1}
                                </div>
                                <div class='reffered-to-location'>
                                    {Doctor-Location-2}
                                </div>
                            </div>
                        </div></div>".Replace("{Doctor-Name}", referral.ProviderFamilyName + " " + string.Join(" ", referral.ProviderGivenNames)).Replace("{Doctor-Speciality}", referral.RefVisitType).Replace("{Doctor-Location-1}", referral.LocationAddressLine1).Replace("{Doctor-Location-2}", referral.LocationAddressLine2).Replace("{Schedule-Day}", referral.AptScheduleDate.Value.DayOfWeek.ToString().Substring(0, 3)).Replace("{Schedule-Month}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(referral.AptScheduleDate.Value.Month).Substring(0, 3)).Replace("{Schedule-Date}", referral.AptScheduleDate.Value.DateTime.Date.Day.ToString()).Replace("{Schedule-Year}", referral.AptScheduleDate.Value.Year.ToString()).Replace("{Schedule-Time}", DateTimeHelper.ConvertTo12HourFormat(referral.BookingSlot));
                }

                labSectionStart += container + " </div> ";
                referralsContainerStart += labSectionStart + "</div>" + "</div>";
                body += referralsContainerStart;
            }

            body += @"<div class='padded-white pb-16 bottom-radius' style='text-align:center;'>
            <div class='referral-text'>
                Please call<span class='referral-subtext referral-inner-text'>630-469-9200</span> to schedule an appointment for the referrals or visit <a href='https://www.dulyhealthandcare.com/services' target='_blank'>https://www.dulyhealthandcare.com/services</a> to make an appointment
            </div>
        </div></div>";

            Console.WriteLine("Referrals Done");
            return body;
        }
    }
}
