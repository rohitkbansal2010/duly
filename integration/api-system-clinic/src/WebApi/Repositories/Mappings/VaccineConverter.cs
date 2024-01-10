// <copyright file="VaccineConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
using AutoMapper;
using Duly.Clinic.Contracts;
using Hl7.Fhir.Model;
using System.Linq;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps https://fhir.epic.com/Specifications?api=1071#1ParamType217931 into <see cref="Vaccine"/>.
    /// </summary>
    public class VaccineConverter : ITypeConverter<CodeableConcept, Vaccine>
    {
        private const string CvxType = "http://hl7.org/fhir/sid/cvx";

        public Vaccine Convert(CodeableConcept source, Vaccine destination, ResolutionContext context)
        {
            return new Vaccine
            {
                Text = source.Text,
                CvxCodes = source.Coding
                    .Where(c => c.System == CvxType)
                    .Select(c => c.Code)
                    .ToArray()
            };
        }
    }
}