namespace Duly.UI.Flourish.GeneratePDF.Model
{
    public class PostSMSStatus
    {
        public string AppointmentId { get; set; }
        public string PatientId { get; set; }
        public string PhoneNumber { get; set; }
        public string SMSStatus { get; set; }
        public string AppointmentTime { get; set; }
        public string AfterVisitPDF { get; set; }
    }
}