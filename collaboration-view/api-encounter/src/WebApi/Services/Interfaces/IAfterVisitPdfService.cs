// <copyright file="IAfterVisitPdfService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.AfterVisitPdf"/> entity.
    /// </summary>
    public interface IAfterVisitPdfService
    {
        /// <summary>
        /// Returns Imaging Detail response.
        /// </summary>
        /// <param name="request"><see cref="Contracts.AfterVisitPdf"/>ImagingDetail.</param>
        /// <returns>Returns cvCheckOut_ID.</returns>
        Task<int> PostAfterVisitPdfAsync(AfterVisitPdf request);

        /// <summary>
        /// Returns after visit pdf id in response.
        /// </summary>
        /// <returns>Returns the after visit pdfs.</returns>
        /// <param name="aftervisitpdfid">Id of cvAfterVisitPDF.</param>
        Task<string> GetAfterVisitPdfById(long aftervisitpdfid);

        /// <summary>
        /// Returns after visit pdf id in response.
        /// </summary>
        /// <returns>Returns the after visit pdf id.</returns>
        /// <param name="id">Id of cvAfterVisitPDF.</param>
        /// <param name="isSMSSent">IsSMSSent of .</param>
        Task<long> UpdateAfterVisitPdfIsSMSSentAsync(long id, bool isSMSSent);
    }
}
