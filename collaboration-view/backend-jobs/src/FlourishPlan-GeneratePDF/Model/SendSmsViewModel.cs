using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.UI.Flourish.GeneratePDF.Model
{
    public class SendSmsViewModel
    {
        [Required]
        [Description("Appointment Id")]
        public string AppointmentId { get; set; }

        [Required]
        [Description("Patient Id")]
        public string PatientId { get; set; }

        [Required]
        [Description("Pdf Id")]
        public string PdfId { get; set; }

        [Description("Phone no")]
        public string PhoneNumber { get; set; }
    }
}