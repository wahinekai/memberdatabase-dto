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
    using System.Linq;
    using System.Threading.Tasks;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using WahineKai.Common;
    using WahineKai.MemberDatabase.Dto.Contracts;
    using WahineKai.MemberDatabase.Dto.Models;
    using WahineKai.MemberDatabase.Dto.Properties;

    /// <summary>
    /// Implementaion of IUserRepository
    /// </summary>
    public sealed class CosmosUserRepository : CosmosRepositoryBase, IUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosUserRepository"/> class.
        /// </summary>
        /// <param name="cosmosConfiguration">Configuration to create connection with an Azure Cosmos DB Database</param>
        /// <param name="loggerFactory">Logger factory to create a logger</param>
        public CosmosUserRepository(CosmosConfiguration cosmosConfiguration, ILoggerFactory loggerFactory)
            : base(cosmosConfiguration, loggerFactory)
        {
        }

        /// <inheritdoc/>
        public async Task<AdminUser> GetUserByEmailAsync(string email)
        {
            // Sanity check input
            email = Ensure.IsNotNullOrWhitespace(() => email);

            this.Logger.LogTrace($"Getting user with email {email} from Cosmos DB");

            using var db = this.GetCosmosContext();

            var maybeNullUser = await db.Users
                .Where(user => user.Email == email)
                .SingleAsync();

            // Ensure user is not null
            var user = Ensure.IsNotNull(() => maybeNullUser);
            user.Validate();

            this.Logger.LogInformation("Got 1 user from Cosmos DB");

            return user;
        }

        /// <inheritdoc/>
        public async Task<AdminUser> GetUserByIdAsync(Guid id)
        {
            // Sanity check input
            id = Ensure.IsNotNull(() => id);

            this.Logger.LogTrace($"Getting user with id {id} from Cosmos DB");

            using var db = this.GetCosmosContext();

            var maybeNullUser = await db.Users
                .Where(user => user.Id == id)
                .SingleAsync();

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

            using var db = this.GetCosmosContext();

            // Get user from database
            var user = await db.Users
                .Where(user => user.Id == id)
                .SingleAsync();

            // Remove user and save
            db.Remove(user);
            await db.SaveChangesAsync();

            this.Logger.LogInformation("Deleted 1 user from the database");
        }

        /// <inheritdoc/>
        public async Task<ICollection<AdminUser>> GetAllUsersAsync()
        {
            this.Logger.LogDebug("Getting all users from Cosmos DB");

            using var db = this.GetCosmosContext();
            return await db.Users.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ICollection<AdminUser>> GetUsersByQueryAsync(string query)
        {
            // Sanity check input
            query = Ensure.IsNotNullOrWhitespace(() => query);
            query = query.ToLower();

            // Get list of all queries
            var queries = query.Split();

            this.Logger.LogDebug($"Getting users matching query \"{query}\" from Cosmos DB");

            var predicate = PredicateBuilder.New<AdminUser>();

            // Add all predicates
            foreach (var q in queries)
            {
                predicate = predicate.Or(user => user.FirstName != null && user.FirstName.ToLower().Contains(q));
                predicate = predicate.Or(user => user.LastName != null && user.LastName.ToLower().Contains(q));
                predicate = predicate.Or(user => user.FacebookName != null && user.FacebookName.ToLower().Contains(q));
                predicate = predicate.Or(user => user.City != null && user.City.ToLower().Contains(q));
                predicate = predicate.Or(user => user.Region != null && user.Region.ToLower().Contains(q));
                predicate = predicate.Or(user => user.Occupation != null && user.Occupation.ToLower().Contains(q));
            }

            using var db = this.GetCosmosContext();
            var users = await db.Users.Where(predicate).ToListAsync();

            this.Logger.LogInformation($"Got {users.Count} users from Cosmos DB");

            return users;
        }

        /// <inheritdoc/>
        public async Task<AdminUser> CreateUserAsync(AdminUser user)
        {
            // Input sanity checking
            user = Ensure.IsNotNull(() => user);
            user.Validate();

            this.Logger.LogTrace($"Checking to see if there already is a user with email {user.Email}");

            using var db = this.GetCosmosContext();

            var existingUserCount = await db.Users.Where(dbUser => dbUser.Email == user.Email).CountAsync();

            if (existingUserCount > 0)
            {
                throw new ArgumentException("Email is already in the database");
            }

            this.Logger.LogTrace($"Creating user with id {user.Id} and email {user.Email} in Cosmos DB");

            // Add user to the database
            var entry = await db.AddAsync(user);
            db.SaveChanges();

            // Check that the user has been added and is valid
            var userFromDatabase = entry.Entity;
            userFromDatabase = Ensure.IsNotNull(() => userFromDatabase);
            userFromDatabase.Validate();

            this.Logger.LogInformation("Created 1 user in the database");

            return userFromDatabase;
        }

        /// <inheritdoc/>
        public async Task<AdminUser> UpdateUserAsync<T>(T updatedUser)
            where T : UserBase
        {
            // Input sanity checking
            updatedUser = Ensure.IsNotNull(() => updatedUser);
            updatedUser.Validate();

            this.Logger.LogTrace($"Checking to see if there already is a user with email {updatedUser.Email}");

            using var db = this.GetCosmosContext();

            var invalidUsersCount = await db.Users
                .Where(user => user.Email == updatedUser.Email && user.Id != updatedUser.Id)
                .CountAsync();

            if (invalidUsersCount > 0)
            {
                throw new ArgumentException("Another user has that Email!");
            }

            this.Logger.LogTrace($"Updating user with id {updatedUser.Id} with new information");

            var user = await db.Users.Where(user => user.Id == updatedUser.Id).SingleAsync();
            user.Update(updatedUser);

            // Check that the user has been added and is valid
            user.Validate();

            // Save changes in the database
            await db.SaveChangesAsync();

            this.Logger.LogInformation("Updated 1 user in the database");

            return user;
        }
    }
}
