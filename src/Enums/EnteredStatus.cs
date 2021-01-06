// -----------------------------------------------------------------------
// <copyright file="EnteredStatus.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Enums
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Various statuses possible about being entered into specific systems
    /// </summary>
    public enum EnteredStatus
    {
        /// <summary>
        /// The user has not been entered in the system
        /// </summary>
        [EnumMember(Value = "Not Entered")]
        NotEntered,

        /// <summary>
        /// The user has been entered in the system, but has not yet accepted the invitation
        /// </summary>
        Entered,

        /// <summary>
        /// The user has accepted their invitation to the system
        /// </summary>
        Accepted,
    }
}
