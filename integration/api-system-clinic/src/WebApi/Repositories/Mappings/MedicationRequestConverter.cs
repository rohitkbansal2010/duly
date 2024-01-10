// <copyright file="MedicationRequestConverter.cs" company="Duly Health and Care">
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
    ///     <see cref="PractitionerWithRoles"/>
    /// into <see cref="Contracts.Medication"/>.
    /// </summary>
    public class MedicationRequestConverter : ITypeConverter<MedicationRequestWithCompartments, Medication>
    {
        public Medication Convert(MedicationRequestWithCompartments source, Medication destination, ResolutionContext context)
        {
            var statement = source.Resource;

            return new Medication
            {
                Id = statement.Id,
                Drug = FindDrug(statement.Medication),
                Period = FindPeriod(statement.DispenseRequest.ValidityPeriod, context.Mapper),
                Status = context.Mapper.Map<MedicationStatus>(statement.Status),
                //Dosages = context.Mapper.Map<Dosage[]>(statement.DosageInstruction),
                Dosages = BuildDosage(source, context.Mapper),
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

        private Dosage[] BuildDosage(MedicationRequestWithCompartments source, IRuntimeMapper contextMapper)
        {
            var statement = source.Resource.DosageInstruction;
            var dosageres = new List<Dosage>();

            foreach (var item in statement)
            {
                Dosage res = new Dosage();
                res.AsNeeded = bool.Parse(item.AsNeeded.ToString());
                //res.Timing = contextMapper.Map<Timing>(item.Timing);
               //res.Timing = buildTiming(item.Timing, contextMapper);
                foreach(var quan in item.DoseAndRate)
                {
                    if(quan.Type.Text == "ordered")
                    {
                        res.DoseQuantity = FindQuantity(quan.Dose, contextMapper);
                        break;
                    }
                }

                res.PatientInstruction = item.PatientInstruction;
                res.Text = item.Text;

                dosageres.Add(res);
            }

            return dosageres.ToArray();
        }

        private static Timing buildTiming(STU3.DataType timing, IRuntimeMapper contextMapper)
        {
            return contextMapper.Map<Timing>(timing);
        }

        private static Contracts.Quantity FindQuantity(STU3.DataType dose, IRuntimeMapper contextMapper)
        {
            return dose switch
            {
                null => default,
                Hl7.Fhir.Model.Quantity quantity => contextMapper.Map<Contracts.Quantity>(quantity),
                _ => default //TODO refactor to handle Hl7.Fhir.Model.Range
            };
        }
    }
}