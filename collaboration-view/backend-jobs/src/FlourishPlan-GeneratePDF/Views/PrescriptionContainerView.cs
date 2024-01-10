using PDFGeneratorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duly.UI.Flourish.GeneratePDF.Views
{
    public static class PrescriptionContainerView
    {
        public static string CreatePrescriptionsContainer(PrescriptionViewModel prescriptions, PharmacyViewModel preferredPharmacy)
        {
            var body = "";

            if(prescriptions == null || prescriptions.Regular == null || prescriptions.Regular.Length <= 0)
            {
                return body;
            }

            body += @"<div class='page-break'><div class='header display--flex location--flex location--flex page-break'>
      <svg class='page-break' style='margin-right:0.5rem;'  width='45' height='45' viewBox='0 0 102 102' fill='none' xmlns='http://www.w3.org/2000/svg'><circle cx='51' cy='51' r='51' fill='#00A5DF' fill-opacity='0.3'/><path d='M68.8508 26.7969H33.1508C31.7982 26.7969 30.501 27.2916 29.5445 28.1721C28.5881 29.0527 28.0508 30.247 28.0508 31.4923V36.1878C28.0508 37.4331 28.5881 38.6274 29.5445 39.508C30.501 40.3886 31.7982 40.8833 33.1508 40.8833V66.7083C33.1508 68.5763 33.9568 70.3678 35.3914 71.6886C36.8261 73.0095 38.7719 73.7515 40.8008 73.7515H61.2008C63.2297 73.7515 65.1755 73.0095 66.6101 71.6886C68.0448 70.3678 68.8508 68.5763 68.8508 66.7083V40.8833C70.2034 40.8833 71.5006 40.3886 72.457 39.508C73.4135 38.6274 73.9508 37.4331 73.9508 36.1878V31.4923C73.9508 30.247 73.4135 29.0527 72.457 28.1721C71.5006 27.2916 70.2034 26.7969 68.8508 26.7969ZM63.7508 59.6651H48.4508V50.2742H63.7508V59.6651ZM63.7508 45.5787H45.9008C45.2245 45.5787 44.5759 45.8261 44.0977 46.2664C43.6194 46.7067 43.3508 47.3038 43.3508 47.9265V62.0129C43.3508 62.6355 43.6194 63.2327 44.0977 63.673C44.5759 64.1133 45.2245 64.3606 45.9008 64.3606H63.7508V66.7083C63.7508 67.331 63.4821 67.9282 63.0039 68.3685C62.5257 68.8087 61.8771 69.0561 61.2008 69.0561H40.8008C40.1245 69.0561 39.4759 68.8087 38.9977 68.3685C38.5194 67.9282 38.2508 67.331 38.2508 66.7083V40.8833H63.7508V45.5787ZM33.1508 36.1878V31.4923H68.8508V36.1878H33.1508Z' fill='#00A5DF'/></svg>
      <div class='patient-detail display--flex page-break'>
        <span class='font--weight--600 font--size--20'>Prescriptions</span>
        <span class='subTitle'>{Prescriptions-Count} new prescriptions have been sent to your pharmacy</span>
      </div>
    </div>".Replace("{Prescriptions-Count}", prescriptions.Regular.Length.ToString());

            var prescriptionContainerStart = @"<div class='display--flex padded-white bottom-radius' style='padding-bottom: 10px;'>";
            var prescriptionSectionStart = @"<div class='lab-section'>";

            int count = 0;
            foreach (var prescription in prescriptions.Regular)
            {
                var providerName = "";
                if (prescription.Provider.HumanName.prefixes[0] != null)
                {
                    providerName += prescription.Provider.HumanName.prefixes[0] + " ";
                }
                if (prescription.Provider.HumanName.givenNames != null)
                {
                    foreach (var givenName in prescription.Provider.HumanName.givenNames)
                    {
                        if (givenName.Length != 0)
                        {
                            providerName += givenName + " ";
                            break;
                        }
                    }
                }
                providerName += prescription.Provider.HumanName.familyName;

                //var providerImage = "";
                //if (prescription.Provider != null && prescription.Provider.Photo != null && (prescription.Provider.Photo.Url != null || prescription.Provider.Photo.Url.Length != 0))
                //{
                //    providerImage = prescription.Provider.Photo.Url;
                //}
                //else
                //{
                //    providerImage = "data:image/png;base64," + System.Environment.GetEnvironmentVariable("default_profile_pic");
                //}
                if(count == 2)
                {
                    prescriptionSectionStart += " </div> ";
                }

                prescriptionSectionStart += @"<div class='details-box page-break'>
          <div
            class='display--flex align--items--center font--weight--600 font--size--14 pending-status-css'>
            <span>{Prescription-Title}</span>
            <span class='circular-text'>RX</span>
          </div>
          <div class='display--flex details-info font--weight--600 pa--2 border-bottom'>
            <div class='display--flex flex--column location--flex'>
              <span class='details-info-title'>PRESCRIBER</span>
              <span class='display--flex align--items--center font--size--14 font--weight-700 pa--0 location--flex'>
                {Provider-Name}</span>
            </div>

            <div class='display--flex flex--column instruction--width location--flex'>
              <span class='details-info-title'>INSTRUCTION</span>
              <span class='font--size--14 font--weight-700'>{Prescription-Instruction}</span>
            </div>
          </div>
        </div>".Replace("{Prescription-Title}", prescription.Title[0].ToString().ToUpper() + prescription.Title.Substring(1, prescription.Title.Length - 1)).Replace("{Provider-Image}", prescription.Provider.Photo.Url).Replace("{Provider-Name}", providerName).Replace("{Prescription-Instruction}", prescription.Instructions);
            }

            prescriptionSectionStart += " </div> ";

            if (preferredPharmacy != null && preferredPharmacy.PharmacyName != null)
            {

                var pharmacyName = preferredPharmacy.PharmacyName.Split(" ")[0];
                prescriptionSectionStart += @"<div class='page-break' style='width:45%;' >
        <div id='map3' class='lab-img' style='width:100%;'> </div>
        <div class='display--flex justify--space--between page-break'>
          <div class='font--size--14 pa--2'>
            <span class='font--weight--600 color-pink' style='font-size:12px;'>YOUR PREFERRED PHARMACY</span><br />
            <span class='font--weight--600' style='line-height:1.5;'>{Pharmacy-Name}</span><br />
            <span style='line-height:1.3;'>{Pharmacy-Address}</span><br />
            <span style='line-height:1.3;'>{Pharmacy-City}, {Pharmacy-State} {Pharmacy-Zip-Code}</span>
          </div>
          <span class='pa--2'>{Pharmacy-Mobile-Number}</span>
        </div>
      </div>".Replace("{Pharmacy-Name}", pharmacyName).Replace("{Pharmacy-Address}", preferredPharmacy.AddressLine1 + " " + (preferredPharmacy.AddressLine2 != "NULL" ? preferredPharmacy.AddressLine2 : "")).Replace("{Pharmacy-City}", preferredPharmacy.City).Replace("{Pharmacy-State}", preferredPharmacy.State).Replace("{Pharmacy-Zip-Code}", preferredPharmacy.ZipCode).Replace("{Pharmacy-Mobile-Number}", preferredPharmacy.PhoneNumber);
            }

            prescriptionContainerStart += prescriptionSectionStart + " </div> ";
            body += prescriptionContainerStart + " </div> ";
            Console.WriteLine("Prescriptions Done");
            return body;
        }
    }
}
