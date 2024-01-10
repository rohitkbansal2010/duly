using System.ComponentModel;

namespace Duly.UI.Flourish.GeneratePDF.Model
{
    public class AfterVisitPdf
    {
        [Description("Patient Id")]
        public string PatientId { get; set; }

        [Description("Provider ID")]
        public string ProviderId { get; set; }

        [Description("After Visit Pdf")]
        public string AfterVisitPDF { get; set; }

        [Description("Appointment Id")]
        public string AppointmentId { get; set; }

        [Description("Trigger SMS")]
        public bool TriggerSMS { get; set; }

        [Description("Phone Number")]
        public string PhoneNumber { get; set; }
    }
}