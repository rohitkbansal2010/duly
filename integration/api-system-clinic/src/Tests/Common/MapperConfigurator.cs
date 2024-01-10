// <copyright file="MapperConfigurator.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using NUnit.Framework;

namespace Duly.Clinic.Api.Tests.Common
{
    public class MapperConfigurator<T>
        where T : Profile, new()
    {
        protected IMapper Mapper { get; set; }

        [SetUp]
        public void SetUp()
        {
            var mappingConfig = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new T());
            });

            Mapper = mappingConfig.CreateMapper();
        }
    }
}
