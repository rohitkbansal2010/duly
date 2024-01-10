// <copyright file="IAfterVisitPdfRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="AfterVisitPdf"/> entities.
    /// </summary>
    public interface IAfterVisitPdfRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="AfterVisitPdf"/> which match with the filter.
        /// </summary>
        /// <param name="request">Identifier of clinic.</param>
        /// <returns>Filtered items of <see cref="AfterVisitPdf"/>.</returns>
        Task<int> PostAfterVisitPdfAsync(AfterVisitPdf request);

        /// <summary>
        /// Get all AfterVisitPdf.
        /// </summary>
        /// <param name="afterVisitPdfId">Identifier of condition targets.</param>
        /// <returns>List items of <see cref="AfterVisitPdf"/>.</returns>
        Task<string> GetAfterVisitPdfByAfterVisitPdfIdAsync(long afterVisitPdfId);
    }
}
