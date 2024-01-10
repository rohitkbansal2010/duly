// <copyright file="NameUse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// The use of a human name.
    /// </summary>
    internal enum NameUse
    {
        /// <summary>
        /// Known as/conventional/the one you normally use.
        /// </summary>
        Usual,

        /// <summary>
        /// The formal name as registered in an official (government) registry, but which name might not be commonly used. May be called "legal name".
        /// </summary>
        Official,

        /// <summary>
        /// A temporary name. Name.period can provide more detailed information. This may also be used for temporary names assigned at birth or in emergency situations.
        /// </summary>
        Temp,

        /// <summary>
        /// A name that is used to address the person in an informal manner, but is not part of their formal or usual name.
        /// </summary>
        Nickname,

        /// <summary>
        /// Anonymous assigned name, alias, or pseudonym (used to protect a person's identity for privacy reasons).
        /// </summary>
        Anonymous,

        /// <summary>
        /// This name is no longer in use (or was never correct, but retained for records).
        /// </summary>
        Old,

        /// <summary>
        /// A name used prior to changing name because of marriage. This name use is for use by applications that collect and store names that were used prior to a marriage. Marriage naming customs vary greatly around the world, and are constantly changing. This term is not gender specific. The use of this term does not imply any particular history for a person's name.
        /// </summary>
        Maiden,
    }
}