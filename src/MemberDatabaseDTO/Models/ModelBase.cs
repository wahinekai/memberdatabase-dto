// -----------------------------------------------------------------------
// <copyright file="ModelBase.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Models
{
    using System;
    using System.Text;
    using Newtonsoft.Json;
    using WahineKai.Common;
    using WahineKai.Common.Contracts;

    /// <summary>
    /// Base class for all models in the system.
    /// All models must inherit (directly or indirectly) from this.
    /// </summary>
    public class ModelBase : IValidatable
    {
        /// <summary>
        /// Gets Azure Cosmos DB id for this user
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public virtual Guid Id { get; init; } = Guid.NewGuid();

        /// <inheritdoc/>
        public void Validate()
        {
            // Must have ID
            Ensure.IsNotNull(() => this.Id);
        }

        /// <summary>
        /// Override of base ToString Method
        /// </summary>
        /// <returns>A printable string representing this document</returns>
        public override string ToString()
        {
            bool valid = true;
            try
            {
                this.Validate();
            }
            catch (Exception)
            {
                valid = false;
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Printing Model");
            stringBuilder.AppendLine("ModelBase Section");
            stringBuilder.AppendLine($"Valid?: {valid}");
            stringBuilder.AppendLine($"id: {this.Id}");

            return stringBuilder.ToString();
        }
    }
}
