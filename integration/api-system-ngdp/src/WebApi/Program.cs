// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.Common.Infrastructure.Components;
using Duly.Common.Infrastructure.Configurations;
using Microsoft.Extensions.Hosting;

namespace Duly.Ngdp.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            DefaultWebHostBuilder
                .Create<Startup>(args, vaultConfigurationKey: ConfigurationBuilderExtensions.GetValueOfVaultConfigurationKeyFromCommandLineArgs(args))
                .Build()
                .Run();
        }
    }
}