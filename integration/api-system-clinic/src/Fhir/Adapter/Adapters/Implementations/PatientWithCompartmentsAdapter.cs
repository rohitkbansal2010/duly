// <copyright file="PatientWithCompartmentsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Extensions;
using Hl7.Fhir.Rest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Adapters.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IPatientWithCompartmentsAdapter"/>
    /// </summary>
    internal class PatientWithCompartmentsAdapter : IPatientWithCompartmentsAdapter
    {
        private readonly IFhirClientR4 _client;
        private readonly IPatientWithCompartmentsBuilder _builder;
        private readonly IPrivateEpicCall _privateEpicCall;

        public PatientWithCompartmentsAdapter(IFhirClientR4 client, IPatientWithCompartmentsBuilder builder, IPrivateEpicCall privateEpicCall)
        {
            _client = client;
            _builder = builder;
            _privateEpicCall = privateEpicCall;
        }

        public async Task<PatientWithCompartments> FindPatientByIdAsync(string patientId)
        {
            //TODO: Replace Search with Read??
            var searchParams = new SearchParams().ById(patientId);
            var searchResult = await _client.FindPatientsAsync(searchParams);
            if (searchResult.Length == 0)
            {
                return null;
            }

            return _builder.ExtractPatientWithCompartments(searchResult);
        }

        public async Task<IEnumerable<PatientWithCompartments>> FindPatientsByIdentifiersAsync(string[] identifiers)
        {
            var searchParams = new SearchParams().ByIdentifiers(identifiers);
            var searchResult = await _client.FindPatientsAsync(searchParams);

            return _builder.ExtractPatientsWithCompartments(searchResult);
        }

        public async Task<GetPatientPhotoRoot> FindPatientPhotoByIdentifiersAsync(PatientPhotoByParam request)
        {
            var result = await _privateEpicCall.GetPatientPhotoAsync(request);
            return result;
        }
    }
}