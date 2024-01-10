// <copyright file="PatientActions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.Ngdp.Adapter.Adapters.Models.CarePlan
{
    /// <summary>
    /// Actions of Targets.
    /// </summary>
    public class PatientActions
    {
        /// <summary>
        /// PatiendTargetId  of patient.
        /// </summary>
        public long PatientTargetId { get; set; }

        /// <summary>
        /// ActionId  of Actions.
        /// </summary>
        public long ActionId { get; set; }

        /// <summary>
        /// Action Type of Action.
        /// </summary>
        public string ActionType { get; set; }

        /// <summary>
        /// Custom Id of the Actions.
        /// </summary>
        public long CustomActionId { get; set; }

        /// <summary>
        /// Deleted.
        /// </summary>
        public bool Deleted { get; set; }
    }
}
