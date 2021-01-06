// -----------------------------------------------------------------------
// <copyright file="CosmosRepositoryBase.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto
{
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using WahineKai.Common;
    using WahineKai.MemberDatabase.Dto.Properties;

    /// <summary>
    /// Base class for repositories interfacing with Cosmos DB
    /// </summary>
    public abstract class CosmosRepositoryBase : RepositoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosRepositoryBase"/> class.
        /// </summary>
        /// <param name="cosmosConfiguration">Configuration to create connection with an Azure Cosmos DB Database</param>
        /// <param name="loggerFactory">Logger factory to create a logger</param>
        public CosmosRepositoryBase(CosmosConfiguration cosmosConfiguration, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.Logger.LogTrace("Construction of Cosmos Repository Base starting");

            // Validate input arguments
            cosmosConfiguration = Ensure.IsNotNull(() => cosmosConfiguration);
            cosmosConfiguration.Validate();

            // Set database id
            this.DatabaseId = Ensure.IsNotNullOrWhitespace(() => cosmosConfiguration.DatabaseId);

            // Create custom JSON serializer for enums
            var jsonSerializationSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            jsonSerializationSettings.Converters.Add(new StringEnumConverter());

            // Create cosmos client
            var cosmosOptions = new CosmosClientOptions() { Serializer = new CosmosJsonSerializer(jsonSerializationSettings) };
            this.CosmosClient = new CosmosClient(cosmosConfiguration.EndpointUrl, cosmosConfiguration.PrimaryKey, cosmosOptions);

            this.Logger.LogTrace("Construction of Cosmos Repository Base complete");
        }

        /// <summary>
        /// Gets Cosmos DB Configuration
        /// </summary>
        protected string DatabaseId { get; }

        /// <summary>
        /// Gets client for interacting with Cosmos DB
        /// </summary>
        protected CosmosClient CosmosClient { get; }
    }
}
