// <copyright file="PatientConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts;
using System;
using System.Collections.Generic;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps http://hl7.org/fhir/patient.html
    /// into <see cref="Patient"/>.
    /// </summary>
    public class PatientConverter : ITypeConverter<PatientWithCompartments, Patient>
    {
        public Patient Convert(PatientWithCompartments source, Patient destination, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.Resource.BirthDate))
                throw new ConceptNotMappedException("BirthDate is a mandatory field");

            if (!source.Resource.Gender.HasValue)
                throw new ConceptNotMappedException("Gender is a mandatory field");

            var patient = new Patient
            {
                BirthDate = context.Mapper.Map<DateTime>(source.Resource.BirthDate),
                Gender = context.Mapper.Map<Gender>(source.Resource.Gender),
                PatientGeneralInfo = context.Mapper.Map<PatientGeneralInfo>(source.Resource),
                Identifiers = source.Resource.Identifier.ConvertIdentifiers(),
                Address = BuildAddress(source),
                PhoneNumber = BuildPhoneNumber(source)
            };

            return patient;
        }

        private List<PatientAddress> BuildAddress(PatientWithCompartments source)
        {
            var Address = new List<PatientAddress>();
            foreach (var item in source.Resource.Address)
            {
                var address = new PatientAddress();
                address.Use = item.Use.ToString();
                address.Line = item.Line;
                address.City = item.City;
                address.PostalCode = item.PostalCode;
                address.Country = item.Country;
                address.State = item.State;
                address.District = item.District;

                Address.Add(address);
            }

            return Address;
        }

        private List<PhoneNumber> BuildPhoneNumber(PatientWithCompartments source)
        {
            var PhoneNumber = new List<PhoneNumber>();
            foreach (var item in source.Resource.Telecom)
            {
                if (item.System.ToString().ToLower() == "phone")
                {
                    var res = new PhoneNumber();
                    res.PhoneNum = item.Value.ToString();
                    res.Use = item.Use.ToString();
                    PhoneNumber.Add(res);
                }
            }

            return PhoneNumber;
        }
    }
}
