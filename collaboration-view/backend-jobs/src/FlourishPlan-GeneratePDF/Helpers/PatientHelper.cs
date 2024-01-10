using PDFGeneratorApp.ViewModel;

namespace Duly.UI.Flourish.GeneratePDF.Helpers
{
    public static class PatientHelper
    {
        public static string GetName(PatientViewModel patient)
        {
            string patientName = "";
            if (patient != null && patient.GeneralInfo != null && patient.GeneralInfo.HumanName != null)
            {
                if (patient.GeneralInfo.HumanName.givenNames != null)
                {
                    foreach (var givenName in patient.GeneralInfo.HumanName.givenNames)
                    {
                        if (givenName.Length != 0)
                        {
                            patientName += givenName + " ";
                        }
                    }
                }
                patientName += patient.GeneralInfo.HumanName.familyName;
            }

            return patientName;
        }

        public static string GetImage(PatientViewModel patient)
        {
            string patientImage = null;
            if (patient.Photo != null)
            {
                if (patient.Photo.Count > 0)
                {
                    patientImage = @"data:image/png;base64," + patient.Photo[0].Data;
                }
            }
            return patientImage;
        }

        public static string GetAddress(PatientViewModel patient)
        {
            var patientHomeAdress = new PatientAddress();
            foreach (var address in patient.PatientAddress)
            {
                if (address.Use == "Home")
                {
                    patientHomeAdress = address;
                }
            }
            var patientAddress = "";
            foreach (var ad in patientHomeAdress.Line)
            {
                patientAddress += ad + " ";
            }
            patientAddress += patientHomeAdress.City + " " + patientHomeAdress.District + " " + patientHomeAdress.State + " " + patientHomeAdress.Country;

            return patientAddress;
        }
    }
}