// <copyright file="MedicationConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias stu3;

using AutoMapper;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts;
using System.Collections.Generic;
using System.Linq;

using STU3 = Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps
    ///     https://www.hl7.org/fhir/medicationstatement.html
    ///     <see cref="PractitionerWithRolesSTU3"/>
    /// into <see cref="Contracts.Medication"/>.
    /// </summary>
    public class MedicationConverter : ITypeConverter<MedicationStatementWithCompartments, Medication>
    {
        public Medication Convert(MedicationStatementWithCompartments source, Medication destination, ResolutionContext context)
        {
            var statement = source.Resource;

            return new Medication
            {
                Id = statement.Id,
                Drug = FindDrug(statement.Medication),
                Period = FindPeriod(statement.Effective, context.Mapper),
                Status = context.Mapper.Map<MedicationStatus>(statement.Status),
                Dosages = context.Mapper.Map<Dosage[]>(statement.Dosage),
                Reason = FindReason(statement.ReasonReference),
                Prescriber = context.Mapper.Map<PractitionerGeneralInfo>(source.Practitioner)
            };
        }

        private static Drug FindDrug(STU3.DataType medication)
        {
            var resourceReference = (STU3.ResourceReference)medication;
            return new Drug
            {
                Title = resourceReference.Display
            };
        }

        private static MedicationReason FindReason(IReadOnlyCollection<STU3.ResourceReference> reasonReference)
        {
            if (reasonReference.Count == 0)
                return null;

            return new MedicationReason
            {
                ReasonText = reasonReference.Select(reference => reference.Display).ToArray()
            };
        }

        private static Period FindPeriod(STU3.DataType statementEffective, IRuntimeMapper contextMapper)
        {
            return statementEffective switch
            {
                null => default,
                STU3.Period period => contextMapper.Map<Period>(period),
                _ => throw new ConceptNotMappedException("Could not map effective of medication statement")
            };
        }
    }
}