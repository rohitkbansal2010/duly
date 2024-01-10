// <copyright file="ConditionConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using AutoMapper;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Extensions.Fhir;
using System.Linq;
using FhirModel = Hl7.Fhir.Model;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps R4.Condition into Condition.
    /// </summary>
    public class ConditionConverter : ITypeConverter<R4.Condition, Condition>
    {
        private const string ClinicalStatusCodeSystem = "http://terminology.hl7.org/CodeSystem/condition-clinical";
        private const string ClinicalStatusVersion = "4.0.0";

        public Condition Convert(R4.Condition source, Condition destination, ResolutionContext context)
        {
            return new Condition
            {
                Id = source.Id,
                AbatementPeriod = ConvertAbatement(source.Abatement, context.Mapper),
                ClinicalStatus = ConvertClinicalStatus(source.ClinicalStatus),
                Name = source.Code.Text,
                RecordedDate = source.RecordedDateElement.BuildDateTimeOffset()
            };
        }

        private static ConditionClinicalStatus ConvertClinicalStatus(FhirModel.CodeableConcept clinicalStatus)
        {
            var code = clinicalStatus?.Coding
                .Where(x => x.System == ClinicalStatusCodeSystem &&
                            x.Version == ClinicalStatusVersion)
                .Select(x => x.Code)
                .ToArray();

            if (code == null || code.Length == 0)
            {
                throw new ConceptNotMappedException("Can't map condition without clinical status");
            }

            if (code.Length > 1)
            {
                throw new ConceptNotMappedException("Ambiguous codes of clinical status");
            }

            return code.Single() switch
            {
                "active" => ConditionClinicalStatus.Active,
                "inactive" => ConditionClinicalStatus.Inactive,
                "resolved" => ConditionClinicalStatus.Resolved,
                _ => throw new ConceptNotMappedException("Unsupported clinical status"),
            };
        }

        private static Period ConvertAbatement(FhirModel.DataType abatement, IRuntimeMapper contextMapper)
        {
            return abatement switch
            {
                null => default,
                FhirModel.Period period => contextMapper.Map<Period>(period),
                _ => throw new ConceptNotMappedException("Could not map Abatement of Condition")
            };
        }
    }
}