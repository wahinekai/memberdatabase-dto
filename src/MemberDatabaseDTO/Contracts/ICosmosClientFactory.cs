// -----------------------------------------------------------------------
// <copyright file="ICosmosClientFactory.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Contracts
{
    using Microsoft.Azure.Cosmos;

    /// <summary>
    /// Interface for the cosmos client factory
    /// </summary>
    public interface ICosmosClientFactory
    {
        /// <summary>
        /// Gets a re-used Cosmos Client unique to only the connection string.
        /// </summary>
        /// <param name="connectionString">The connection string to create the client with</param>
        /// <returns>A <see cref="CosmosClient"/></returns>
        public CosmosClient GetCosmosClient(string connectionString);
    }
}