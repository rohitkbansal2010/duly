using Microsoft.Extensions.Configuration;
using PDFGeneratorApp.ViewModel;

namespace Duly.UI.Flourish.GeneratePDF.Helpers
{
    public static class DoctorHelper
    {
        public static string GetDoctorImage(FollowUpViewModel followUpData)
        {
            string doctorImage = null;
            if (followUpData != null && (followUpData.photo != null) && (followUpData.photo.url != null || followUpData.photo.url != ""))
            {
                doctorImage = followUpData.photo.url;
            }

            return doctorImage;
        }
    }
}