using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PDFGeneratorApp.ViewModel
{
    public class PatientViewModel
    {
        public PatientGeneralInfo GeneralInfo { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public List<Attachment> Photo { get; set; }

        public List<PatientAddress> PatientAddress { get; set; }

        public List<PhoneNumber> PhoneNumber { get; set; }
    }

    public class PatientGeneralInfo
    {
        [Description("An identifier for the patient")]
        public string Id { get; set; }

        [Description("A name associated with the patient")]
        public HumanName HumanName { get; set; }
    }

    public enum Gender
    {
        [Description("Male")]
        Male,

        [Description("Female")]
        Female,

        [Description("Other")]
        Other,

        [Description("Unknown")]
        Unknown
    }

    public class PhoneNumber
    {
        public string PhoneNum { get; set; }
        public string Use { get; set; }
    }

    public class PatientAddress
    {
        [Description("use")]
        public string Use { get; set; }

        [Description("Street address")]
        public IEnumerable<string> Line { get; set; }

        [Description("City")]
        public string City { get; set; }

        [Description("Code for mail service")]
        public string PostalCode { get; set; }

        [Description("Part of country")]
        public string State { get; set; }

        [Description("Country")]
        public string Country { get; set; }

        [Description("Part of county")]
        public string District { get; set; }
    }

    public class Attachment
    {
        [Description("Mime type of the content")]
        public string ContentType { get; set; }

        [Description("Label to display in place of the data")]
        public string Title { get; set; }

        [Description("Number of bytes of content (if url provided)")]
        public int Size { get; set; }

        [Description("Uri where the data can be found")]
        public string Url { get; set; }

        [Description("Data inline, base64ed")]
        public string Data { get; set; }
    }
}