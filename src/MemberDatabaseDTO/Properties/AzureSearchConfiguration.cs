// -----------------------------------------------------------------------
// <copyright file="AzureSearchConfiguration.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Properties
{
    using System;
    using Azure;
    using Microsoft.Extensions.Configuration;
    using WahineKai.Common;
    using WahineKai.Common.Contracts;

    /// <summary>
    /// Configuration for connection to Azure Blob Storage
    /// </summary>
    public class AzureSearchConfiguration : IValidatable
    {
        /// <summary>
        /// Gets the endpoint for the Azure Search Configuration
        /// </summary>
        public Uri? Endpoint { get; init; }

        /// <summary>
        /// Gets the name of the Azure Search Index
        /// </summary>
        public string? IndexName { get; init; }

        /// <summary>
        /// Gets the Azure Key Credential Needed to Complete this Request
        /// </summary>
        public AzureKeyCredential? AzureKeyCredential { get; init; }

        /// <summary>
        /// Gets the name of the suggester to work with
        /// </summary>
        public string? SuggesterName { get; init; }

        /// <summary>
        /// Gets a value determining whether this configuration can perform suggestions and auto-completes
        /// </summary>
        public bool? CanSuggest { get; init; }

        /// <summary>
        /// Builds an Azure Blob configuration from a configuration
        /// </summary>
        /// <param name="configuration">Dotnet configuration object</param>
        /// <returns>A validated Azure Blob configuration. Throws if not possible.</returns>
        public static AzureSearchConfiguration BuildFromConfiguration(IConfiguration configuration)
        {
            var suggesterName = configuration["AzureSearch:SuggesterName"];

            // Can suggest if a suggester name was provided
            var canSuggest = suggesterName != null;

            // Build CosmosConfiguration
            var azureSearchConfiguration = new AzureSearchConfiguration
            {
                Endpoint = new Uri(configuration["AzureSearch:Endpoint"]),
                IndexName = configuration["AzureSearch:IndexName"],
                AzureKeyCredential = new AzureKeyCredential(configuration["AzureSearch:ServiceKey"]),
                SuggesterName = suggesterName,
                CanSuggest = canSuggest,
            };

            // Validate Azure Blob Configuration
            azureSearchConfiguration.Validate();

            return azureSearchConfiguration;
        }

        /// <inheritdoc/>
        public void Validate()
        {
            Ensure.IsNotNull(() => this.Endpoint);
            Ensure.IsNotNullOrWhitespace(() => this.IndexName);
            Ensure.IsNotNull(() => this.AzureKeyCredential);
            Ensure.IsNotNull(() => this.CanSuggest);
        }
    }
}
