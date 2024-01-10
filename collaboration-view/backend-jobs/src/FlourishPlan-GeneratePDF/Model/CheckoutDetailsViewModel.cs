using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PDFGeneratorApp.ViewModel
{
    public class CheckoutDetailsViewModel
    {
        [Description("LabDetailsList")]
        public IEnumerable<GetLabOrImaging> LabDetailsList { get; set; }
        [Description("ScheduleFollowUpList")]
        public IEnumerable<ScheduleReferral> ScheduleFollowUpList { get; set; }
        [Description("PrescriptionIsReviewed")]
        public bool PrescriptionIsReviewed { get; set; }
        [Description("ReferralIsReviewed")]
        public bool ReferralIsReviewed { get; set; }
        [Description("Message")]
        public string Message { get; set; }
    }

    public class GetLabOrImaging
    {
        [Description("ID")]
        public int ID { get; set; }
        [Description("Type")]
        public string Type { get; set; }
        [Description("Lab_ID")]
        public string Lab_ID { get; set; }
        [Description("Lab_Location")]
        public string Lab_Location { get; set; }
        [Description("Lab_Name")]
        public string Lab_Name { get; set; }
        [Description("CreatedDate")]
        public DateTimeOffset CreatedDate { get; set; }
        [Description("Appointment_ID")]
        public string Appointment_ID { get; set; }
        [Description("Patient_ID")]
        public string Patient_ID { get; set; }
        [Description("Provider_ID")]
        public string Provider_ID { get; set; }
        [Description("Location_ID")]
        public string Location_ID { get; set; }
        [Description("BookingSlot")]
        public string BookingSlot { get; set; }
        [Description("AptScheduleDate")]
        public DateTimeOffset AptScheduleDate { get; set; }
        [Description("ImagingLocation")]
        public string ImagingLocation { get; set; }
        [Description("ImagingType")]
        public string ImagingType { get; set; }
        [Description("Skipped")]
        public bool Skipped { get; set; }
    }

    public class ScheduleReferral
    {
        [Description("ID")]
        public int ID { get; set; }
        [Description("Provider_ID")]
        public string Provider_ID { get; set; }
        [Description("Patient_ID")]
        public string Patient_ID { get; set; }
        [Description("AptType")]
        public string AptType { get; set; }
        [Description("AptFormat")]
        public string AptFormat { get; set; }
        [Description("Location_ID")]
        public string Location_ID { get; set; }
        [Description("AptScheduleDate")]
        public DateTimeOffset? AptScheduleDate { get; set; }
        [Description("BookingSlot")]
        public string BookingSlot { get; set; }
        [Description("RefVisitType")]
        public string RefVisitType { get; set; }
        [Description("Created_Date")]
        public DateTimeOffset? Created_Date { get; set; }
        [Description("Type")]
        public string Type { get; set; }
        [Description("Appointment_Id")]
        public string Appointment_Id { get; set; }
        [Description("Skipped")]
        public bool Skipped { get; set; }
        [Description("Department_Id")]
        public string Department_Id { get; set; }
        [Description("VisitTypeId")]
        public string VisitTypeId { get; set; }
        [Description("LocationAddressLine1")]
        public string LocationAddressLine1 { get; set; }
        [Description("LocationAddressLine2")]
        public string LocationAddressLine2 { get; set; }
        [Description("ProviderGivenNames")]
        public string[] ProviderGivenNames { get; set; }
        [Description("ProviderFamilyName")]
        public string ProviderFamilyName { get; set; }
    }
}