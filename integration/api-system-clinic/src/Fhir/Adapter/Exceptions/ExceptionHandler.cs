// <copyright file="ExceptionHandler.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
extern alias stu3;

using System;

using R4 = r4::Hl7.Fhir.Rest;
using STU3 = stu3::Hl7.Fhir.Rest;

namespace Duly.Clinic.Fhir.Adapter.Exceptions
{
    public static class ExceptionHandler
    {
        public static int? ExtendHandling(Exception exception)
        {
            return exception switch
            {
                R4.FhirOperationException fhirOperationExceptionR4 => (int)fhirOperationExceptionR4.Status,
                STU3.FhirOperationException fhirOperationExceptionSTU3 => (int)fhirOperationExceptionSTU3.Status,
                _ => null
            };
        }
    }
}
