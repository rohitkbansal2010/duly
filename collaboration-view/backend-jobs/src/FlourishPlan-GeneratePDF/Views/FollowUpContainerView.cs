using Duly.UI.Flourish.GeneratePDF.Helpers;
using Duly.UI.Flourish.GeneratePDF.Model;
using PDFGeneratorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duly.UI.Flourish.GeneratePDF.Views
{
    public static class FollowUpContainerView
    {
        public static string CreateFollowUpContainer(CheckoutDetailsViewModel checkoutData, List<FollowUpViewModel> followUpData, PatientViewModel patient, AppointmentDetailsBySMSStatusViewModel pendingData, SitesDetailsViewModel followUpLocation, string doctorImage)
        {
            string container = "";

            var flag = false;
            if (checkoutData.ScheduleFollowUpList != null || checkoutData.ScheduleFollowUpList.Count() > 0)
            {

                foreach (var followUp in checkoutData.ScheduleFollowUpList)
                {
                    if (followUp.Type == "F" && Convert.ToBoolean(followUp.Skipped) == false)
                    {
                        flag = true;
                        container += @" <div class='follow-container'> ";
                        container += @"<div class='follow-up-header display--flex location--flex'><svg style='margin-right:0.5rem;'  width='45' height='45' viewBox='0 0 65 65' fill='none' xmlns='http://www.w3.org/2000/svg'><circle cx='32.2887' cy='32.2887' r='32.2887' fill='#B3E4F5'/><path d='M43.5889 17.3634H41.9072V15.6817C41.9072 15.2357 41.73 14.8079 41.4146 14.4926C41.0992 14.1772 40.6715 14 40.2255 14C39.7795 14 39.3517 14.1772 39.0363 14.4926C38.7209 14.8079 38.5438 15.2357 38.5438 15.6817V17.3634H25.0902V15.6817C25.0902 15.2357 24.913 14.8079 24.5976 14.4926C24.2823 14.1772 23.8545 14 23.4085 14C22.9625 14 22.5347 14.1772 22.2194 14.4926C21.904 14.8079 21.7268 15.2357 21.7268 15.6817V17.3634H20.0451C18.7071 17.3634 17.4238 17.8949 16.4777 18.8411C15.5315 19.7872 15 21.0705 15 22.4085V45.9523C15 47.2904 15.5315 48.5736 16.4777 49.5197C17.4238 50.4659 18.7071 50.9974 20.0451 50.9974H43.5889C44.9269 50.9974 46.2102 50.4659 47.1563 49.5197C48.1024 48.5736 48.634 47.2904 48.634 45.9523V22.4085C48.634 21.0705 48.1024 19.7872 47.1563 18.8411C46.2102 17.8949 44.9269 17.3634 43.5889 17.3634ZM45.2706 45.9523C45.2706 46.3983 45.0934 46.8261 44.778 47.1415C44.4626 47.4568 44.0349 47.634 43.5889 47.634H20.0451C19.5991 47.634 19.1713 47.4568 18.856 47.1415C18.5406 46.8261 18.3634 46.3983 18.3634 45.9523V30.817H45.2706V45.9523ZM45.2706 27.4536H18.3634V22.4085C18.3634 21.9625 18.5406 21.5347 18.856 21.2194C19.1713 20.904 19.5991 20.7268 20.0451 20.7268H21.7268V22.4085C21.7268 22.8545 21.904 23.2823 22.2194 23.5976C22.5347 23.913 22.9625 24.0902 23.4085 24.0902C23.8545 24.0902 24.2823 23.913 24.5976 23.5976C24.913 23.2823 25.0902 22.8545 25.0902 22.4085V20.7268H38.5438V22.4085C38.5438 22.8545 38.7209 23.2823 39.0363 23.5976C39.3517 23.913 39.7795 24.0902 40.2255 24.0902C40.6715 24.0902 41.0992 23.913 41.4146 23.5976C41.73 23.2823 41.9072 22.8545 41.9072 22.4085V20.7268H43.5889C44.0349 20.7268 44.4626 20.904 44.778 21.2194C45.0934 21.5347 45.2706 21.9625 45.2706 22.4085V27.4536Z' fill='#00A5DF'/><path d='M26.5273 38.5302L29.8911 42.3032C30.31 42.7731 31.0537 42.7436 31.4342 42.2421L37.5273 34.2102' stroke='#00A5DF' stroke-width='3' stroke-linecap='round'/></svg><div class='patient-detail display--flex'><span class='font--weight--600 font--size--20'>Follow-Up Appointments</span></div></div> ".Replace("{Follow-Up-Appointment-Date-Time}", pendingData.AppointmentTime);




                        container += @" <div class='follow-up-details page-break'>
        <div id='follow-up-heading' style='background-color:white;'>
          <span style='color: #0f172a; font-style: normal; font-weight: 700;' class='family-inter'>
              &nbsp;{Schedule-Day}, {Schedule-Month} {Schedule-Date}, {Schedule-Year} - {Schedule-Time}
          </span>
          <span style='display:flex; align-items: center; margin-left: 6rem;'>
              <img class='border-radius-css' height='30px' width='30px' src='{Doctor-Image-Url}' alt=''>
              <p  style='font-style: normal; font-weight: 700;' class='family-inter'>&nbsp;{Doctor-Name}</p>
          </span>
        </div>

        <div class='display--flex details-info font--weight--600 border-bottom page-break'>
          <div class='display--flex flex--column location--flex' style='margin:1rem;'>
            <span class='details-info-title'>APPOINTMENT TYPE</span>
            <span class='font--size--14 font--weight-700'>{Appointment-Type}</span>
          </div>
          <div class='display--flex flex--column location--flex' style='margin:1rem;'>
            <span class='details-info-title'>APPOINTMENT FORMAT</span>
            <span class='font--size--14 font--weight-700'>{Appointment-Format}</span>
          </div>
          <div class='display--flex flex--column location--flex' style='margin:1rem;'>
            <span class='details-info-title'>LOCATION</span>
            <span class='font--size--14 font--weight-700'>{Doctor-Address-Line1}</span>
            <span class='font--size--14 font--light'>{Doctor-Address-Line2}</span>
          </div>
        </div>
      </div> ".Replace("{Schedule-Day}", followUp.AptScheduleDate.Value.DayOfWeek.ToString().Substring(0, 3)).Replace("{Schedule-Month}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(followUp.AptScheduleDate.Value.Month).Substring(0, 3)).Replace("{Schedule-Date}", followUp.AptScheduleDate.Value.DateTime.ToString().Substring(0, 2).Replace("/","")).Replace("{Schedule-Year}", followUp.AptScheduleDate.Value.Year.ToString()).Replace("{Schedule-Time}", DateTimeHelper.ConvertTo12HourFormat(followUp.BookingSlot)).Replace("{Doctor-Name}", " Dr " + followUpData[0].humanName.familyName).Replace("{Appointment-Type}", followUp.AptType).Replace("{Appointment-Format}", followUp.AptFormat).Replace("{Doctor-Image-Url}", doctorImage).Replace("{Doctor-Address-Line1}", followUpLocation.Line).Replace("{Doctor-Address-Line2}", followUpLocation.City + ", " + followUpLocation.State + ", " + followUpLocation.PostalCode);
                        break;
                    }
                }
            }

            if (!flag)
            {
                container += @"<div class='follow-container'> ";
                container += @"<div class='follow-up-header display--flex location--flex'><svg style='margin-right:0.5rem;'  width='45' height='45' viewBox='0 0 65 65' fill='none' xmlns='http://www.w3.org/2000/svg'><circle cx='32.2887' cy='32.2887' r='32.2887' fill='#B3E4F5'/><path d='M43.5889 17.3634H41.9072V15.6817C41.9072 15.2357 41.73 14.8079 41.4146 14.4926C41.0992 14.1772 40.6715 14 40.2255 14C39.7795 14 39.3517 14.1772 39.0363 14.4926C38.7209 14.8079 38.5438 15.2357 38.5438 15.6817V17.3634H25.0902V15.6817C25.0902 15.2357 24.913 14.8079 24.5976 14.4926C24.2823 14.1772 23.8545 14 23.4085 14C22.9625 14 22.5347 14.1772 22.2194 14.4926C21.904 14.8079 21.7268 15.2357 21.7268 15.6817V17.3634H20.0451C18.7071 17.3634 17.4238 17.8949 16.4777 18.8411C15.5315 19.7872 15 21.0705 15 22.4085V45.9523C15 47.2904 15.5315 48.5736 16.4777 49.5197C17.4238 50.4659 18.7071 50.9974 20.0451 50.9974H43.5889C44.9269 50.9974 46.2102 50.4659 47.1563 49.5197C48.1024 48.5736 48.634 47.2904 48.634 45.9523V22.4085C48.634 21.0705 48.1024 19.7872 47.1563 18.8411C46.2102 17.8949 44.9269 17.3634 43.5889 17.3634ZM45.2706 45.9523C45.2706 46.3983 45.0934 46.8261 44.778 47.1415C44.4626 47.4568 44.0349 47.634 43.5889 47.634H20.0451C19.5991 47.634 19.1713 47.4568 18.856 47.1415C18.5406 46.8261 18.3634 46.3983 18.3634 45.9523V30.817H45.2706V45.9523ZM45.2706 27.4536H18.3634V22.4085C18.3634 21.9625 18.5406 21.5347 18.856 21.2194C19.1713 20.904 19.5991 20.7268 20.0451 20.7268H21.7268V22.4085C21.7268 22.8545 21.904 23.2823 22.2194 23.5976C22.5347 23.913 22.9625 24.0902 23.4085 24.0902C23.8545 24.0902 24.2823 23.913 24.5976 23.5976C24.913 23.2823 25.0902 22.8545 25.0902 22.4085V20.7268H38.5438V22.4085C38.5438 22.8545 38.7209 23.2823 39.0363 23.5976C39.3517 23.913 39.7795 24.0902 40.2255 24.0902C40.6715 24.0902 41.0992 23.913 41.4146 23.5976C41.73 23.2823 41.9072 22.8545 41.9072 22.4085V20.7268H43.5889C44.0349 20.7268 44.4626 20.904 44.778 21.2194C45.0934 21.5347 45.2706 21.9625 45.2706 22.4085V27.4536Z' fill='#00A5DF'/><path d='M26.5273 38.5302L29.8911 42.3032C30.31 42.7731 31.0537 42.7436 31.4342 42.2421L37.5273 34.2102' stroke='#00A5DF' stroke-width='3' stroke-linecap='round'/></svg><div class='patient-detail display--flex'><span class='font--weight--600 font--size--20'>Follow-Up Appointments</span><span class='subTitle'>{Follow-Up-Appointment-Date-Time}</span> </div></div> ".Replace("{Follow-Up-Appointment-Date-Time}", "");



                container += @" <div class='follow-up-details page-break'>
        <div id='follow-up-heading' style='background-color:white;'>
          <span class='color-pink font--weight--600 family-inter' style='font-style: normal;'>
              &nbsp;{Follow-Up-Date-Time}
          </span>
          <span style='display:flex; align-items: center; margin-left: 15rem;'>
              <img class='border-radius-css' height='30px' width='30px' src='{Doctor-Image-Url}' >
              <p  style='font-style: normal; font-weight: 700;' class='family-inter'>&nbsp;{Doctor-Name}</p>
          </span>
        </div>

        <div class='display--flex details-info font--weight--600 border-bottom page-break'>
          <div class='display--flex flex--column location--flex' style='margin:1rem;'>
            <span class='details-info-title'>LOCATION</span>
            <span class='font--size--14 font--weight-700'>{Patient-City}</span>
            <span class='font--size--14 font--light'>{Patient-Location}</span>
          </div>

          
         
        </div>
      </div> ".Replace("{Follow-Up-Date-Time}", "Pending").Replace("{Doctor-Name}", " Dr " + followUpData[0].humanName.familyName).Replace("{Patient-City}", followUpLocation.Line).Replace("{Patient-Location}", followUpLocation.City + ", " + followUpLocation.State + ", " + followUpLocation.PostalCode).Replace("{Doctor-Image-Url}", doctorImage);
            }
            Console.WriteLine("Followup is done");
            return container;
        }


    }
}
