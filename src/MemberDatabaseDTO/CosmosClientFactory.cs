// -----------------------------------------------------------------------
// <copyright file="CosmosClientFactory.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Azure.Cosmos;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using WahineKai.Common;
    using WahineKai.MemberDatabase.Dto.Contracts;

    /// <summary>
    /// Base class for repositories interfacing with Cosmos DB
    /// </summary>
    public sealed class CosmosClientFactory : ICosmosClientFactory, IDisposable
    {
        /// <summary>
        /// The options to be used when creating clients in this factory.
        /// </summary>
        private readonly CosmosClientOptions cosmosOptions;

        /// <summary>
        /// Dictionary of existing clients and their connection strings.
        /// </summary>
        private readonly IDictionary<string, CosmosClient> existingClients = new Dictionary<string, CosmosClient>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosClientFactory"/> class.
        /// </summary>
        public CosmosClientFactory()
        {
            // Create custom JSON serializer for enums
            var jsonSerializationSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            jsonSerializationSettings.Converters.Add(new StringEnumConverter());

            // Create cosmos client
            this.cosmosOptions = new CosmosClientOptions() { Serializer = new CosmosJsonSerializer(jsonSerializationSettings) };
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            foreach (var client in this.existingClients.Values)
            {
                client.Dispose();
            }
        }

        /// <inheritdoc/>
        public CosmosClient GetCosmosClient(string connectionString)
        {
            connectionString = Ensure.IsNotNullOrWhitespace(() => connectionString);

            // Return existing cosmos client if it exists
            if (this.existingClients.ContainsKey(connectionString))
            {
                return this.existingClients[connectionString];
            }

            using var newCosmosClient = new CosmosClient(connectionString, this.cosmosOptions);
            this.existingClients.Add(connectionString, newCosmosClient);
            return newCosmosClient;
        }
    }
}
