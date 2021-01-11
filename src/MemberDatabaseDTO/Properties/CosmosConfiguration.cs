// -----------------------------------------------------------------------
// <copyright file="CosmosConfiguration.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Properties
{
    using Microsoft.Extensions.Configuration;
    using WahineKai.Common;
    using WahineKai.Common.Contracts;

    /// <summary>
    /// Configuration needed to connect to Azure Cosmos DB
    /// </summary>
    public class CosmosConfiguration : IValidatable
    {
        /// <summary>
        /// Gets the connection string for the Azure Blob Configuration
        /// </summary>
        public string? ConnectionString { get; init; }

        /// <summary>
        /// Gets the database id of this cosmos configuration
        /// </summary>
        public string? DatabaseId { get; init; }

        /// <summary>
        /// Builds a cosmos configuration from a configuration
        /// </summary>
        /// <param name="configuration">Dotnet configuration object</param>
        /// <returns>A validated Cosmos configuration. Throws if not possible.</returns>
        public static CosmosConfiguration BuildFromConfiguration(IConfiguration configuration)
        {
            // Build CosmosConfiguration
            var cosmosConfiguration = new CosmosConfiguration
            {
                ConnectionString = configuration["Cosmos:ConnectionString"],
                DatabaseId = configuration["Cosmos:DatabaseId"],
            };

            // Validate Cosmos Configuration
            cosmosConfiguration.Validate();

            return cosmosConfiguration;
        }

        /// <inheritdoc/>
        public void Validate()
        {
            Ensure.IsNotNullOrWhitespace(() => this.ConnectionString);
            Ensure.IsNotNullOrWhitespace(() => this.DatabaseId);
        }
    }
}
