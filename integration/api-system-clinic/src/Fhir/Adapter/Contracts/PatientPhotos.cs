// <copyright file="PatientPhotos.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Duly.Clinic.Fhir.Adapter.Contracts
{
    public class PatientPhotos
    {
        public string ApprovalApplication { get; set; }
        public DateTime? ApprovalDateTime { get; set; }
        public byte[] Data { get; set; }
        public string FileExtension { get; set; }
        public int FileSize { get; set; }
        public bool IsMyCPhotoPending { get; set; }
        public bool IsVisibleInMyChart { get; set; }
        public int OriginalFileSize { get; set; }
        public int PhotoHeight { get; set; }
        public string PhotoSource { get; set; }
        public string PhotoStatus { get; set; }
        public int PhotoWidth { get; set; }
        public string ResizeCode { get; set; }
    }
}