// -----------------------------------------------------------------------
// <copyright file="MemberStatus.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Enums
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Various statuses a member can be in
    /// </summary>
    public enum MemberStatus
    {
        /// <summary>
        /// A member has asked to join Wahine Kai, but has not yet officially joined the club.
        /// This member doesn't have a joined, renewal, or terminated date.
        /// </summary>
        Pending,

        /// <summary>
        /// This member is an active, dues-paying member.
        /// This is a normal member.
        /// This type of member must have a join and renewal date, but no terminated date.
        /// </summary>
        [EnumMember(Value = "Active: Paying")]
        ActivePaying,

        /// <summary>
        /// This member is an active member that does not pay dues.
        /// Examples of this member might be honorary members or current board members.
        /// This type of member must have a join date, but not a renewal or terminated date.
        /// </summary>
        [EnumMember(Value = "Active: Non-Paying")]
        ActiveNonPaying,

        /// <summary>
        /// This member is an active member of the club who will never pay dues.
        /// A member might become a lifetime member by holding a board seat for some years,
        /// or by providing other special services to the club.
        /// This member must have a joined date, but not a renewal or terminated date.
        /// </summary>
        [EnumMember(Value = "Lifetime Member")]
        LifetimeMember,

        /// <summary>
        /// This is a member who has terminated their membership.
        /// This member cannot access any Wahine Kai functions.
        /// This member must have a join and terminated date, but will not have a renewal date.
        /// </summary>
        Terminated,
    }
}
