// <copyright file="PatientPhotoConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    public class PatientPhotoConverter : ITypeConverter<Models.PatientPhoto, Attachment>
    {
        public Attachment Convert(Models.PatientPhoto source, Attachment destination, ResolutionContext context)
        {
            var photo = new Attachment
            {
                Title = source.Title,
                ContentType = source.FileExtension,
                Size = source.FileSize,
                Data = source.Photo
            };

            return photo;
        }
    }
}