using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace PDFGeneratorApp.ViewModel
{
    public class PrescriptionViewModel
    {
        [Description("Information about regular medications that the patient is/was taking.")]
        public Medication[] Regular { get; set; }

        [Description("Information about other medications that the patient is/was taking.")]
        public Medication[] Other { get; set; }
    }

    public class Medication
    {
        [Description("An identifier for the medication")]
        public string Id { get; set; }

        [Description("Medication schedule type")]
        public MedicationScheduleType ScheduleType { get; set; }

        [Description("The name of the medicine, including the brand name, active ingredient and dosage.")]
        public string Title { get; set; }

        [Description("A reason why the medication the patient is/was taking.")]
        public string Reason { get; set; }

        [Description("The date the patient started taking the medicine.")]
        public DateTimeOffset? StartDate { get; set; }

        [Description("The person that provided the information about the taking of this medication.")]
        public PractitionerGeneralInfo Provider { get; set; }

        [Description("The patient instructions for the prescription.")]
        public string Instructions { get; set; }
    }

    public enum MedicationScheduleType
    {
        [Description("Regular scheduled medications")]
        Regular,

        [Description("Other")]
        Other
    }

    public class PractitionerGeneralInfo
    {
        [Description("Practitioner Id")]
        public string Id { get; set; }

        [Description("Name of a Practitioner with parts and usage")]
        public HumanName HumanName { get; set; }

        [Description("Image of a Practitioner")]
        public Attachment Photo { get; set; }

        [Description("Role of a Practitioner")]
        public PractitionerRole Role { get; set; }

        [Description("Speciality of a Practitioner")]
        public List<string> Speciality { get; set; }
    }

    public enum PractitionerRole
    {
        [Description("Medical assistent (MA)")]
        [EnumMember(Value = "MA")]
        MedicalAssistant,

        [Description("Primary care physician (PCP)")]
        [EnumMember(Value = "PCP")]
        PrimaryCarePhysician,

        [Description("Could not identify the role")]
        [EnumMember(Value = "Unknown")]
        Unknown
    }
}