// -----------------------------------------------------------------------
// <copyright file="CosmosUserRepository.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Azure.Cosmos.Linq;
    using Microsoft.Extensions.Logging;
    using WahineKai.Common;
    using WahineKai.MemberDatabase.Dto.Contracts;
    using WahineKai.MemberDatabase.Dto.Models;
    using WahineKai.MemberDatabase.Dto.Properties;

    /// <summary>
    /// Implementaion of IUserRepository
    /// </summary>
    /// <typeparam name="T">The type of user to return</typeparam>
    public sealed class CosmosUserRepository<T> : CosmosRepositoryBase, IUserRepository<T>
        where T : ReadByAllUser
    {
        private readonly Microsoft.Azure.Cosmos.Container container;

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosUserRepository{T}"/> class.
        /// </summary>
        /// <param name="cosmosConfiguration">Configuration to create connection with an Azure Cosmos DB Database</param>
        /// <param name="loggerFactory">Logger factory to create a logger</param>
        public CosmosUserRepository(CosmosConfiguration cosmosConfiguration, ILoggerFactory loggerFactory)
            : base(cosmosConfiguration, loggerFactory)
        {
            this.Logger.LogTrace("Beginning construction of Cosmos User Repository");

            this.container = this.CosmosClient.GetContainer(this.DatabaseId, UserBase.ContainerId);

            this.Logger.LogTrace("Construction of Cosmos User Repository complete");
        }

        /// <inheritdoc/>
        public async Task<T> GetUserByEmailAsync(string email)
        {
            // Sanity check input
            email = Ensure.IsNotNullOrWhitespace(() => email);

            this.Logger.LogTrace($"Getting user with email {email} from Cosmos DB");

            using var iterator = await this.WithRetriesAsync<FeedIterator<T>, CosmosException>(
                () => this.container.GetItemLinqQueryable<T>()
                #pragma warning disable CS8602 // All users in database will have email
                .Where(user => user.Email.Equals(email))
                #pragma warning restore CS8602
                .Take(1)
                .ToFeedIterator());

            var feedResponse = await iterator.ReadNextAsync();

            // There should be only one result
            Ensure.IsFalse(() => iterator.HasMoreResults);
            Ensure.AreEqual(() => 1, () => feedResponse.Count);

            var maybeNullUser = feedResponse.Single(user => user.Email == email);

            // Ensure user is not null
            var user = Ensure.IsNotNull(() => maybeNullUser);
            user.Validate();

            this.Logger.LogInformation("Got 1 user from Cosmos DB");

            return user;
        }

        /// <inheritdoc/>
        public async Task<T> GetUserByIdAsync(Guid id)
        {
            // Sanity check input
            id = Ensure.IsNotNull(() => id);

            this.Logger.LogTrace($"Getting user with id {id} from Cosmos DB");

            using var iterator = await this.WithRetriesAsync<FeedIterator<T>, CosmosException>(
                () => this.container.GetItemLinqQueryable<T>()
                .Where(user => id.ToString() == user.Id.ToString())
                .Take(1)
                .ToFeedIterator());

            var feedResponse = await iterator.ReadNextAsync();

            // There should be only one result
            Ensure.IsFalse(() => iterator.HasMoreResults);
            Ensure.AreEqual(() => 1, () => feedResponse.Count);

            var maybeNullUser = feedResponse.Single(user => id.Equals(user.Id));

            // Ensure user is not null
            var user = Ensure.IsNotNull(() => maybeNullUser);
            user.Validate();

            this.Logger.LogInformation("Got 1 user from Cosmos DB");

            return user;
        }

        /// <inheritdoc/>
        public async Task DeleteUserByIdAsync(Guid id)
        {
            // Sanity check input
            Ensure.IsNotNull(() => id);

            this.Logger.LogTrace($"Deleting user with id {id} from the database");

            await this.WithRetriesAsync<CosmosException>(
                async () => await this.container.DeleteItemAsync<T>(id.ToString(), new Microsoft.Azure.Cosmos.PartitionKey(id.ToString())));

            this.Logger.LogInformation("Deleted 1 user from the database");
        }

        /// <inheritdoc/>
        public async Task<ICollection<T>> GetAllUsersAsync()
        {
            this.Logger.LogDebug("Getting all users from Cosmos DB");

            using var iterator = await this.WithRetriesAsync<FeedIterator<T>, CosmosException>(
                () => this.container.GetItemQueryIterator<T>());

            var users = new Collection<T>();

            while (iterator.HasMoreResults)
            {
                foreach (var user in await iterator.ReadNextAsync())
                {
                    Ensure.IsNotNull(() => user);
                    user.Validate();

                    users.Add(user);
                }
            }

            this.Logger.LogInformation($"Got {users.Count} users from Cosmos DB");

            return users;
        }

        /// <inheritdoc/>
        public async Task<IList<T>> GetUsersByIdCollectionAsync(IEnumerable<Guid> idList)
        {
            // Sanity check input
            idList = Ensure.IsNotNull(() => idList);

            // If empty, return new list
            if (idList.Count() == 0)
            {
                return new List<T>();
            }

            var allUsers = await this.GetAllUsersAsync();

            var usersInSearch = allUsers.Join(idList, user => user.Id, id => id, (user, id) => user).ToList();

            this.Logger.LogInformation($"Got {usersInSearch.Count} users from Cosmos DB");

            return usersInSearch;
        }

        /// <inheritdoc/>
        public async Task<T> CreateUserAsync(T user)
        {
            // Input sanity checking
            user = Ensure.IsNotNull(() => user);
            user.Validate();

            this.Logger.LogTrace($"Checking to see if there already is a user with email {user.Email}");

            try
            {
                using var iterator = await this.WithRetriesAsync<FeedIterator<T>, CosmosException>(
                    () => this.container.GetItemLinqQueryable<T>()
                    .Where(dbUser => dbUser.Email == user.Email)
                    .Take(1)
                    .ToFeedIterator());

                var feedResponse = await iterator.ReadNextAsync();

                // There should be no results
                Ensure.IsFalse(() => iterator.HasMoreResults);
                Ensure.AreEqual(() => 0, () => feedResponse.Count);
            }
            catch (Exception)
            {
                throw new ArgumentException("Email is already in the database");
            }

            this.Logger.LogTrace($"Creating user with id {user.Id} and email {user.Email} in Cosmos DB");

            // Add user to the database
            var userResponse = await this.container.CreateItemAsync(user);

            // Check that the user has been added and is valid
            var userFromDatabase = userResponse.Resource;
            userFromDatabase = Ensure.IsNotNull(() => userFromDatabase);
            userFromDatabase.Validate();

            this.Logger.LogInformation("Created 1 user in the database");

            return userFromDatabase;
        }

        /// <inheritdoc/>
        public async Task<T> ReplaceUserAsync(T updatedUser, Guid id)
        {
            // Input sanity checking
            updatedUser = Ensure.IsNotNull(() => updatedUser);
            updatedUser.Validate();
            id = Ensure.IsNotNull(() => id);

            this.Logger.LogTrace($"Checking to see if there already is a user with email {updatedUser.Email}");

            try
            {
                using var iterator = await this.WithRetriesAsync<FeedIterator<T>, CosmosException>(
                    () => this.container.GetItemLinqQueryable<T>()
                    .Where(dbUser => dbUser.Email == updatedUser.Email)
                    .Take(1)
                    .ToFeedIterator());

                var feedResponse = await iterator.ReadNextAsync();

                // There should be either no results or one user with the same id
                Ensure.IsFalse(() => iterator.HasMoreResults);
                Ensure.IsTrue(() => feedResponse.Count <= 1);

                if (feedResponse.Count == 1)
                {
                    var dbUser = feedResponse.Single(user => user.Email == updatedUser.Email);
                    Ensure.AreEqual(() => updatedUser.Id, () => dbUser.Id);
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("Email is already in the database with another user");
            }

            this.Logger.LogTrace($"Replacing user with id {id} with new information");

            var userResponse = await this.container.ReplaceItemAsync(updatedUser, id.ToString());

            // Check that the user has been added and is valid
            var userFromDatabase = userResponse.Resource;
            userFromDatabase = Ensure.IsNotNull(() => userFromDatabase);
            userFromDatabase.Validate();

            this.Logger.LogInformation("Replaced 1 user in the database");

            return userFromDatabase;
        }
    }
}
