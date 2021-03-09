// -----------------------------------------------------------------------
// <copyright file="CosmosRepositoryBase.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto
{
    using Microsoft.Extensions.Logging;
    using WahineKai.Common;
    using WahineKai.MemberDatabase.Dto.Properties;

    /// <summary>
    /// Base class for repositories interfacing with Cosmos DB
    /// </summary>
    public abstract class CosmosRepositoryBase : RepositoryBase
    {
        /// <summary>
        /// Gets Cosmos Configuration
        /// </summary>
        private readonly CosmosConfiguration cosmosConfiguration;

        /// <summary>
        /// Gets Logger Factory
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

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
            this.loggerFactory = Ensure.IsNotNull(() => loggerFactory);
            this.cosmosConfiguration = Ensure.IsNotNull(() => cosmosConfiguration);
            this.cosmosConfiguration.Validate();

            this.Logger.LogTrace("Construction of Cosmos Repository Base complete");
        }

        /// <summary>
        /// Uses members to get Cosmos Context
        /// </summary>
        /// <returns>Cosmos Context</returns>
        protected CosmosContext GetCosmosContext()
            => new CosmosContext(this.cosmosConfiguration, this.loggerFactory);
    }
}
