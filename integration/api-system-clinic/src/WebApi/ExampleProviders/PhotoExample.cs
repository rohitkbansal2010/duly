// <copyright file="PhotoExample.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using System;
using System.IO;

namespace Duly.Clinic.Api.ExampleProviders
{
    public static class PhotoExample
    {
        public static Attachment MakeAttachment()
        {
            return new Attachment
            {
                Title = "Photo",
                ContentType = "image/x-png",
                Data = Convert.ToBase64String(PhotoExample.ReadPhoto())
            };
        }

        public static byte[] ReadPhoto()
        {
            using var stream = new MemoryStream();

            // See details https://docs.microsoft.com/en-US/dotnet/api/system.drawing.image.save?view=net-5.0
            Properties.Resources.Photo.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return stream.ToArray();
        }
    }
}
