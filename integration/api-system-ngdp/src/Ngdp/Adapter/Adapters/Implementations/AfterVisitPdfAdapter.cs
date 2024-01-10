// <copyright file="AfterVisitPdfAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IAfterVisitPdfAdapter"/>
    /// </summary>
    internal class AfterVisitPdfAdapter : IAfterVisitPdfAdapter
    {
        private const string InsertAfterVisitPdfProcedureName = "[uspInsertAfterVisitPdf]";
        private const string GetAfterVisitPdfProcedureName = "[uspGetAllAfterVisitPdf]";

        private readonly ICVDapperContext _dapperContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AfterVisitPdfAdapter(ICVDapperContext dapperContext, IHttpContextAccessor httpContextAccessor)
        {
            _dapperContext = dapperContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<int> AfterVisitPdfAsync(AfterVisitPdf request)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            dynamic AfterVisitPdf = new
            {
                Patient_ID = request.PatientId,
                Provider_ID = request.ProviderId,
                AfterVisitPDF = request.AfterVisitPDF,
                Appointment_ID = request.AppointmentId,
                Created_By = userName
            };
            return _dapperContext.ExecuteScalarAsync<int>(InsertAfterVisitPdfProcedureName, AfterVisitPdf);
        }

        public Task<string> GetAfterVisitPdfByAfrerVisitPdfIdAsync(long aftervisitpdfId)
        {
            var parameters = new
            {
                ID = aftervisitpdfId
            };

            var result = _dapperContext.QuerySingleAsync<string>(GetAfterVisitPdfProcedureName, parameters);
            return result;
        }
    }
}
