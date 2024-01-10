// <copyright file="RecommendedProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Adapter.Adapters.Models
{
    public class RecommendedProvider
    {
        /// <summary>
        /// Identifier of the referral recommended provider in format '{Referral_Id}_{Pat_Id}_{Referred_Provider_Id}_{Provider_Id}'.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Identifier of the referral.
        /// </summary>
        public decimal? ReferralId { get; set; }

        /// <summary>
        /// Description of the identifier value.
        /// </summary>
        public string ReferralIdType { get; set; }

        /// <summary>
        /// Status of the referral.
        /// </summary>
        public string ReferralStatus { get; set; }

        /// <summary>
        /// Identifier of the patient.
        /// </summary>
        public string PatientId { get; set; }

        /// <summary>
        /// Last name of the patient.
        /// </summary>
        public string PatientLastName { get; set; }

        /// <summary>
        /// First name of the patient.
        /// </summary>
        public string PatientFirstName { get; set; }

        /// <summary>
        /// Patient's date of birth.
        /// </summary>
        public DateTime? PatientDateOfBirth { get; set; }

        /// <summary>
        /// First part of the patient's address.
        /// </summary>
        public string PatientAddressFirstLine { get; set; }

        /// <summary>
        /// Second (last) part of the patient's address.
        /// </summary>
        public string PatientAddressSecondLine { get; set; }

        /// <summary>
        /// Patient's city identifier (name).
        /// </summary>
        public string PatientCity { get; set; }

        /// <summary>
        /// Patient's state identifier.
        /// </summary>
        public string PatientState { get; set; }

        /// <summary>
        /// Patient's zip (postal) code.
        /// </summary>
        public string PatientZip { get; set; }

        /// <summary>
        /// Patient's phone number.
        /// </summary>
        public string PatientPhone { get; set; }

        /// <summary>
        /// Identifier of the specialty.
        /// </summary>
        public string SpecialtyId { get; set; }

        /// <summary>
        /// Description of the specialty id value.
        /// </summary>
        public string SpecialtyIdType { get; set; }

        /// <summary>
        /// Name of the specialty.
        /// </summary>
        public string SpecialtyName { get; set; }

        /// <summary>
        /// Identifier of the referred provider.
        /// </summary>
        public string ReferredToProviderId { get; set; }

        /// <summary>
        /// Description of the ReferredProvider id value.
        /// </summary>
        public string ReferredToProviderIdType { get; set; }

        /// <summary>
        /// Identifier of the department.
        /// </summary>
        public decimal? DepartmentId { get; set; }

        /// <summary>
        /// Description of the department id value.
        /// </summary>
        public string DepartmentIdType { get; set; }

        /// <summary>
        /// Name of the department.
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Department's phone number.
        /// </summary>
        public string DepartmentPhone { get; set; }

        /// <summary>
        /// Identifier of the location.
        /// </summary>
        public decimal? LocationId { get; set; }

        /// <summary>
        /// Description of the location id value.
        /// </summary>
        public string LocationIdType { get; set; }

        /// <summary>
        /// Name of the location.
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// First part of the location address.
        /// </summary>
        public string LocationAddressFirstLine { get; set; }

        /// <summary>
        /// Second (last) part of the location address.
        /// </summary>
        public string LocationAddressSecondLine { get; set; }

        /// <summary>
        /// Location city identifier (name).
        /// </summary>
        public string LocationCity { get; set; }

        /// <summary>
        /// Location state identifier.
        /// </summary>
        public string LocationState { get; set; }

        /// <summary>
        /// Location zip (postal) code.
        /// </summary>
        public string LocationZip { get; set; }

        /// <summary>
        /// Distance between provider location and patient's home.
        /// </summary>
        public double? LocationDistanceFromPatientHome { get; set; }

        /// <summary>
        /// Identifier of the visit type.
        /// </summary>
        public int VisitTypeId { get; set; }

        /// <summary>
        /// Description of the VisitType id value.
        /// </summary>
        public string VisitTypeIdType { get; set; }

        /// <summary>
        /// Name of the VisitType.
        /// </summary>
        public string VisitTypeName { get; set; }

        /// <summary>
        /// Identifier of the provider.
        /// </summary>
        public string ProviderId { get; set; }

        /// <summary>
        /// Description of the provider id value.
        /// </summary>
        public string ProviderIdType { get; set; }

        /// <summary>
        /// Specialty of the provider.
        /// </summary>
        public string ProviderSpecialty { get; set; }

        /// <summary>
        /// Name of the provider.
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// Flag denoting availability of provider's slots in next two weeks.
        /// </summary>
        public string IsSlotAvailableInNextTwoWeeks { get; set; }
    }
}
