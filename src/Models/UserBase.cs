﻿// -----------------------------------------------------------------------
// <copyright file="UserBase.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;
    using WahineKai.Common;
    using WahineKai.Common.Contracts;

    /// <summary>
    /// Base class - includes requried information for all user types
    /// </summary>
    public abstract class UserBase : IValidatable
    {
        /// <summary>
        /// Container Id for this model
        /// </summary>
        public const string ContainerId = "Users";

        /// <summary>
        /// Partion key for this container
        /// </summary>
        public const string PartitionKey = "/id";

        /// <summary>
        /// Gets Azure Cosmos DB id for this user
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public virtual Guid Id { get; init; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets user email address, required
        /// </summary>
        [EmailAddress]
        public string? Email { get; set; }

        /// <inheritdoc/>
        public void Validate()
        {
            // Email is required
            this.Email = Ensure.IsNotNullOrWhitespace(() => this.Email);
        }
    }
}
