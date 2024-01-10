// <copyright file="CustomTargets.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
        /// <summary>
        /// Custom Targets.
        /// </summary>
        public class CustomTargets
        {
            /// <summary>
            /// Target Name of condition targets.
            /// </summary>
            public string TargetName { get; set; }

            /// <summary>
            /// Min value of condition targets.
            /// </summary>
            public string MinValue { get; set; }

            /// <summary>
            /// Max value of condition targets.
            /// </summary>
            public string MaxValue { get; set; }

            /// <summary>
            /// Range.
            /// </summary>
            public string Range { get; set; }

            /// <summary>
            /// UnitOfMeasure.
            /// </summary>
            public string UnitOfMeasure { get; set; }

            /// <summary>
            /// Description of condition targets.
            /// </summary>
            public string Description { get; set; }
        }
}