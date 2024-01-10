// <copyright file="IAfterVisitPdfRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Models.AfterVisitPdf"/> model.
    /// </summary>
    internal interface IAfterVisitPdfRepository
    {
        public Task<int> PostAfterVisitPdfAsync(Models.AfterVisitPdf request);
        public Task<string> GetAfterVisitPdfAsync(long id);
        public Task<long> UpdateAfterVisitPdfIsSMSSentAsync(long id, bool isSMSSent);
    }
}