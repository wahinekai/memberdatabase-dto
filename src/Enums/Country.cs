// -----------------------------------------------------------------------
// <copyright file="Country.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Enums
{
    using System.Runtime.Serialization;
    using StatesAndProvinces;

    /// <summary>
    /// Serializable version of CountrySelection enum
    /// </summary>
    public enum Country
    {
        /// <summary>
        /// Serializable Canada selection
        /// </summary>
        Canada = CountrySelection.Canada,

        /// <summary>
        /// Serializable United States selection
        /// </summary>
        [EnumMember(Value = "United States")]
        UnitedStates = CountrySelection.UnitedStates,
    }
}
