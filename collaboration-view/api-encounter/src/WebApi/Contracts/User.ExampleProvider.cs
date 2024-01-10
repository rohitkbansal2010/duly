// <copyright file="User.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts.Extensions;
using Swashbuckle.AspNetCore.Filters;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class UserExampleProvider : IExamplesProvider<User>
    {
        public User GetExamples()
        {
            return new User
            {
                Id = "sample-user-id",
                Name = new HumanName
                {
                    FamilyName = "Wood",
                    GivenNames = new[] { "John", "Henry" }
                },
                Photo = PhotoExample.MakeAttachment()
            };
        }
    }
}
