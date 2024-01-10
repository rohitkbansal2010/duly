// -----------------------------------------------------------------------
// <copyright file="AfterVisitPdfRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// --------------

using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.Common.DataAccess.Contexts.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IAfterVisitPdfRepository"/>
    /// </summary>
    internal class AfterVisitPdfRepository : IAfterVisitPdfRepository
    {
        private const string InsertAfterVisitPdfProcedureName = "[uspInsertAfterVisitPdf]";
        private const string GetAfterVisitPdfProcedureName = "[uspGetAllAfterVisitPdf]";
        private const string UpdateAfterVisitPdfIsSMSSentProcedureName = "[uspUpdateAfterVisitPdfIsSMSSent]";

        private readonly IDapperContext _dapperContext;

        private readonly string _userName = string.Empty;

        public AfterVisitPdfRepository(
        IDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public Task<int> PostAfterVisitPdfAsync(Models.AfterVisitPdf request)
        {
            int firstCommaIndex = request.AfterVisitPDF.IndexOf(',');
            string afterVisitPdfPart = request.AfterVisitPDF.Substring(firstCommaIndex + 1);
            var binaryDataAfterVisitPdf = Convert.FromBase64String(afterVisitPdfPart);

            dynamic AfterVisitPdf = new
            {
                Patient_ID = request.PatientId,
                Provider_ID = request.ProviderId,
                AfterVisitPDF = binaryDataAfterVisitPdf,
                Appointment_ID = request.AppointmentId,
                PhoneNumber = request.PhoneNumber,
                Created_By = _userName
            };
            return _dapperContext.ExecuteScalarAsync<int>(InsertAfterVisitPdfProcedureName, AfterVisitPdf);
        }

        public async Task<string> GetAfterVisitPdfAsync(long id)
        {
            var parameters = new
            {
                ID = id
            };

            var afterVisitPdf = await _dapperContext.QuerySingleAsync<byte[]>(GetAfterVisitPdfProcedureName, parameters);
            string afterVisitPdfInfoPart = "data:application/pdf;filename=generated.pdf;base64,";
            var orginalPdf = Convert.ToBase64String(afterVisitPdf);
            var pdfResponse = orginalPdf.Insert(0, afterVisitPdfInfoPart);
            return pdfResponse;
        }

        public Task<long> UpdateAfterVisitPdfIsSMSSentAsync(long id, bool isSMSSent)
        {
            var parameters = new
            {
                Id = id,
                IsSMSSent = isSMSSent,
                UpdatedBy = _userName
            };

            var afterVisitPdf = _dapperContext.QueryFirstOrDefaultAsync<long>(UpdateAfterVisitPdfIsSMSSentProcedureName, parameters);
            return afterVisitPdf;
        }
    }
}