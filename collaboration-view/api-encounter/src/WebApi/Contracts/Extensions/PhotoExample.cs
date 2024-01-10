// <copyright file="PhotoExample.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.IO;

namespace Duly.CollaborationView.Encounter.Api.Contracts.Extensions
{
    public static class PhotoExample
    {
        public enum Photo
        {
            Photo1,
            Photo2
        }

        public static Attachment MakeAttachment(Photo photo = Photo.Photo1)
        {
            return new Attachment
            {
                Title = "Photo",
                ContentType = "image/x-png",
                Data = Convert.ToBase64String(ReadPhoto(photo))
            };
        }

        public static byte[] ReadPhoto(Photo photo = Photo.Photo1)
        {
            using var stream = new MemoryStream();

            switch (photo)
            {
                case Photo.Photo1:
                    Properties.Resources.Photo.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                    break;
                case Photo.Photo2:
                    Properties.Resources.Photo2.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    break;
            }

            // See details https://docs.microsoft.com/en-US/dotnet/api/system.drawing.image.save?view=net-5.0
            return stream.ToArray();
        }
    }
}