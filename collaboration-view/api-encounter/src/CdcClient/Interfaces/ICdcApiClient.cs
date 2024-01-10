// <copyright file="ICdcApiClient.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.CdcClient.Models;
using System.Threading.Tasks;

namespace Duly.CollaborationView.CdcClient.Interfaces
{
    /// <summary>
    /// Client to communicate with CDC REST API.
    /// https://vaccinecodeset.cdc.gov/SymedicalCDCVCABPRODRuntimeRestService/swagger/ui/index
    /// </summary>
    public interface ICdcApiClient
    {
        /// <summary>
        /// Returns all <see cref="CdcContentModelCatalog"/> associated with a published content model.
        /// </summary>
        /// <param name="contentModel">The content model mnemonic.</param>
        /// <returns>An array of <see cref="CdcContentModelCatalog"/>.</returns>
        Task<CdcContentModelCatalog[]> GetContentModelCatalogsAsync(string contentModel);

        /// <summary>
        /// Returns all <see cref="CdcCatalogSourceCodeRelations"/>, each containing a collection of
        /// <see cref="CdcRelatedTermItem"/> for source codes requested.
        /// </summary>
        /// <param name="contentModel">The content model mnemonic.</param>
        /// <param name="catalog">Catalog uid or mnemonic for term.</param>
        /// <param name="codes">Array of term source codes.</param>
        /// <returns>An array of <see cref="CdcCatalogSourceCodeRelations"/>.</returns>
        Task<CdcCatalogSourceCodeRelations[]> FindContentModelCatalogRelationsByCodesAsync(string contentModel, string catalog, string[] codes);
    }
}
