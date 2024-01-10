using System.Collections.Generic;
using System.ComponentModel;

namespace PDFGeneratorApp.ViewModel
{
    public class OpenReferralViewModel
    {
        [Description("Speciality")]
        public IEnumerable<OpenReferralResponse> OpenRefferalResponse { get; set; }

        [Description("Referred By Id")]
        public string Message { get; set; }

        [Description("Referred Date")]
        public string StatusCode { get; set; }
    }

    public class OpenReferralResponse
    {
        [Description("Speciality")]
        public string Speciality { get; set; }

        [Description("Referred By Name")]
        public string ReferredByName { get; set; }

        [Description("Referred By Id")]
        public string ReferredById { get; set; }

        [Description("Referred Date")]
        public string ReferredDate { get; set; }

        [Description("Is Scheduled")]
        public bool IsScheduled { get; set; }
    }
}