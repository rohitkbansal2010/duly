using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class PostPatientTargetsResponse
    {
        public int PatientConditionId { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
