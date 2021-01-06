// -----------------------------------------------------------------------
// <copyright file="AzureBlobConfiguration.cs" company="Wahine Kai">
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
    /// Configuration for connection to Azure Blob Storage
    /// </summary>
    public class AzureBlobConfiguration : IValidatable
    {
        /// <summary>
        /// Gets the connection string for the Azure Blob Configuration
        /// </summary>
        public string? ConnectionString { get; init; }

        /// <summary>
        /// Builds an Azure Blob configuration from a configuration
        /// </summary>
        /// <param name="configuration">Dotnet configuration object</param>
        /// <returns>A validated Azure Blob configuration. Throws if not possible.</returns>
        public static AzureBlobConfiguration BuildFromConfiguration(IConfiguration configuration)
        {
            // Build CosmosConfiguration
            var azureBlobConfiguration = new AzureBlobConfiguration
            {
                ConnectionString = configuration["AzureBlob:ConnectionString"],
            };

            // Validate Azure Blob Configuration
            azureBlobConfiguration.Validate();

            return azureBlobConfiguration;
        }

        /// <inheritdoc/>
        public void Validate()
        {
            Ensure.IsNotNullOrWhitespace(() => this.ConnectionString);
        }
    }
}
