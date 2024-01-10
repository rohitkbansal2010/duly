using System.Collections.Generic;

namespace PDFGeneratorApp.ViewModel
{
    public class ImaginServiceSlugConversion
    {
        Dictionary<string, string> serviceSlugDictionary = new Dictionary<string, string> {
                    {
            "Annual Screening Mammogram", "annual-screening-mammogram"
          },
          {
            "Radiology", "radiology"
               },
          {
            "CT", "ct"
          },
          {
            "DEXA", "dexa"
          },
          {
            "Fluoroscopy", "fluoroscopy"
          },
          {
            "MRI", "mri"
          },
          {
            "Nuclear Medicine/PET", "nuclear-medicine-pet"
          },
          {
            "PET/CT", "pet-ct"
          },
          {
            "Ultrasound", "ultrasound"
          },
          {
            "X-Ray", "x-ray"
          },
          {
            "Interventional Radiology", "interventional-radiology"
          },
          {
              "breast services", "breast-services"
          }

           };
    }
}