// <copyright file="MapperConfigurator.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using NUnit.Framework;
using System;

namespace Duly.Ngdp.Api.Tests.Common
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
                conf.ConstructServicesUsing(ConstructService);
            });

            Mapper = mappingConfig.CreateMapper();
        }

        protected virtual object ConstructService(Type serviceType)
        {
            throw new NotImplementedException($"Implement this method for {serviceType.Name} in derived class.");
        }
    }
}
