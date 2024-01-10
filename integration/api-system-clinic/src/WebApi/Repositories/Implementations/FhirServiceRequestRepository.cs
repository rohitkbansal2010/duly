// <copyright file="FhirServiceRequestRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IServiceRequestRepository"/>
    /// </summary>
    internal class FhirServiceRequestRepository : IServiceRequestRepository
    {
        private readonly IServiceRequestWithCompartmentsAdapter _adapter;
        private readonly IMapper _mapper;

        public FhirServiceRequestRepository(
            IServiceRequestWithCompartmentsAdapter adapter,
            IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<ServiceRequest> GetLabOrdersAsync(string patientId, string appointmentId, string type)
        {
            var Category = string.Empty;
            if(type.ToLower() == "labs")
            {
                Category = "urn:oid:1.2.840.114350.1.13.687.3.7.10.798268.30|1";
            }
            else if(type.ToLower() == "imaging")
            {
                Category = "urn:oid:1.2.840.114350.1.13.687.3.7.10.798268.30|13";
            }
            else
            {
                throw new System.Exception("Category Not Defined");
            }

            var searchCriteria = new ServicerequestSearchCriteria
            {
                PatientReference = patientId,
                Status = "active",
                Category = Category
            };
            var fhirServiceRequest = await _adapter.FindLabImagingOrdersAsync(searchCriteria);
            List<Orders> result = new List<Orders>();
            if(fhirServiceRequest != null) 
            {
                foreach(var item in fhirServiceRequest)
                {
                    if (item.Resource.Encounter.Identifier != null && item.Resource.Encounter.Identifier.Value == appointmentId)
                    {
                        var order = new Orders();
                        order.OrderName = item.Resource.Code.Text;
                        order.AuthoredOn = item.Resource.AuthoredOn;
                        result.Add(order);
                    }
                }
            }

            var systemServiceRequest = new ServiceRequest
            {
                TestOrder = result,
                OrderCount = result.Count
            };
            return systemServiceRequest;
        }
    }
}
