// -----------------------------------------------------------------------
// <copyright file="SearchResult.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Models
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Model for return from a search
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Gets or sets Azure Cosmos DB id for this user
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public virtual Guid Id { get; set; }
    }
}
