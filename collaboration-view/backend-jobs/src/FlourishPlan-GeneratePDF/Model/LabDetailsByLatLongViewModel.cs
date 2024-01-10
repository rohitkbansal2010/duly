using System.ComponentModel;

namespace PDFGeneratorApp.ViewModel
{
    public class LabDetailsByLatLongViewModel
    {
        [Description("Lab Id")]
        public int LabId { get; set; }

        [Description("Lab Name")]
        public string LabName { get; set; }

        [Description("ExternalLabYn")]
        public string ExternalLabYn { get; set; }

        [Description("LabLlnId")]
        public string LabLlbId { get; set; }

        [Description("LlbName")]
        public string LlbName { get; set; }

        [Description("Lab Address line 1")]
        public string LlbAddressLn1 { get; set; }

        [Description("Lab Address line 2")]
        public string LlbAddressLn2 { get; set; }

        [Description("Lab Location city")]
        public string LLCity { get; set; }

        [Description("Lab Location state")]
        public string LLState { get; set; }

        [Description("Lab Location Zip")]
        public string LLZip { get; set; }

        [Description("Lab Location latitude")]
        public double LabLatitude { get; set; }

        [Description("Lab Location longitude")]
        public string LabLongitude { get; set; }

        [Description("Distance")]
        public string Distance { get; set; }

        [Description("Contact Number")]
        public string ContactNumber { get; set; }

        [Description("Working Hours")]
        public string WorkingHours { get; set; }
    }
}