// <copyright file="AppointmentByCsnIdConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Api.Client;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps <see cref="RecommendedProvider"/> into <see cref="Models.Provider"/>.
    /// </summary>
    public class AppointmentByCsnIdConverter :
        ITypeConverter<Ngdp.Api.Client.DepartmentVisitType, Models.AppointmentByCsnId>
    {
        public Models.AppointmentByCsnId Convert(DepartmentVisitType source, Models.AppointmentByCsnId destination, ResolutionContext context)
        {
            return new Models.AppointmentByCsnId
            {
                VisitTypeId = "External|" + source.VisitTypeId,
                DepartmentId = "External|" + source.DepartmentId,
                PatientId = "External|" + source.PatientId,
                ProviderId = "External|" + source.ProviderId
            };
        }
    }
}
