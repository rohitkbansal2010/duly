// <copyright file="IAfterVisitPdfAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces
{
    /// <summary>
    /// An adapter for working with "AfterVisitPdf" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface IAfterVisitPdfAdapter
    {
        /// <summary>
        /// Finds matching items of <see cref="AfterVisitPdf"/>.
        /// </summary>
        /// <param name="request">Criteria to search by.</param>
        /// <returns>Filtered items of <see cref="AfterVisitPdf"/>.</returns>
        Task<int> AfterVisitPdfAsync(AfterVisitPdf request);

        /// <summary>
        /// Get all after visit pdf by aftervisitpdf Id ids.
        /// </summary>
        /// <param name="aftervisitpdfid">Identifier of condition targets.</param>
        /// <returns>List items of <see cref="AfterVisitPdf"/>.</returns>
        Task<string> GetAfterVisitPdfByAfrerVisitPdfIdAsync(long aftervisitpdfid);
    }
}
