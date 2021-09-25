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
    using WahineKai.Common;
    using WahineKai.MemberDatabase.Dto.Contracts;
    using WahineKai.MemberDatabase.Dto.Properties;

    /// <summary>
    /// Base class for repositories interfacing with Cosmos DB
    /// </summary>
    public abstract class CosmosRepositoryBase : RepositoryBase
    {
        /// <summary>
        /// Shared cosmos client factory for creating cosmos clients
        /// </summary>
        private static readonly ICosmosClientFactory CosmosClientFactory = new CosmosClientFactory();

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

            this.CosmosClient = CosmosRepositoryBase
                .CosmosClientFactory
                .GetCosmosClient(Ensure.IsNotNullOrWhitespace(() => cosmosConfiguration.ConnectionString));

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
