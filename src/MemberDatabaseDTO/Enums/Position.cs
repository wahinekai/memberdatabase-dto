// -----------------------------------------------------------------------
// <copyright file="Position.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Enums
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Positions that can be held by Wahine Kai Members
    /// </summary>
    public enum Position
    {
        /// <summary>
        /// President of the club
        /// </summary>
        President,

        /// <summary>
        /// Vice President of the club
        /// </summary>
        [EnumMember(Value = "Vice President")]
        VicePresident,

        /// <summary>
        /// Vice President of events of the club
        /// </summary>
        [EnumMember(Value = "Vice President of Events")]
        VicePresidentOfEvents,

        /// <summary>
        /// Vice President of finance of the club
        /// </summary>
        [EnumMember(Value = "Vice President of Finance")]
        VicePresidentOfFinance,

        /// <summary>
        /// Director of marketing for the club
        /// </summary>
        [EnumMember(Value = "Director of Marketing")]
        DirectorOfMarketing,

        /// <summary>
        /// Director of social media for the club
        /// </summary>
        [EnumMember(Value = "Director of Social Media")]
        DirectorOfSocialMedia,

        /// <summary>
        /// Director of community services for the club
        /// </summary>
        [EnumMember(Value = "Director of Community Services")]
        DirectorOfCommunityServices,

        /// <summary>
        /// Surf mama director for the club
        /// </summary>
        [EnumMember(Value = "Surf Mama Director")]
        SurfMamaDirector,

        /// <summary>
        /// Director of a club chapter
        /// </summary>
        [EnumMember(Value = "Chapter Director")]
        ChapterDirector,

        /// <summary>
        /// Event coordinator for a club chapter
        /// </summary>
        [EnumMember(Value = "Chapter Event Coordinator")]
        ChapterEventCoordinator,

        /// <summary>
        /// Merchandiser for the club
        /// </summary>
        Merchandiser,
    }
}
