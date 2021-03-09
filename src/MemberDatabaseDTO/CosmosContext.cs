// -----------------------------------------------------------------------
// <copyright file="CosmosContext.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using Microsoft.Extensions.Logging;
    using WahineKai.Common;
    using WahineKai.MemberDatabase.Dto.Enums;
    using WahineKai.MemberDatabase.Dto.Models;
    using WahineKai.MemberDatabase.Dto.Properties;

    /// <summary>
    /// Base class for repositories interfacing with Cosmos DB
    /// </summary>
    public sealed class CosmosContext : DbContext
    {
        /// <summary>
        /// Gets logger for logging all transactions
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Gets Azure Cosmos DB Connection String
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Gets Azure Cosmos DB database Name
        /// </summary>
        private readonly string databaseName;

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosContext"/> class.
        /// </summary>
        /// <param name="cosmosConfiguration">Configuration to create connection with an Azure Cosmos DB Database</param>
        /// <param name="loggerFactory">Logger factory to create a logger</param>
        #pragma warning disable CS8618 // Field is filled in by configuration
        public CosmosContext(CosmosConfiguration cosmosConfiguration, ILoggerFactory loggerFactory)
        #pragma warning restore CS8618
        {
            loggerFactory = Ensure.IsNotNull(() => loggerFactory);
            this.logger = loggerFactory.CreateLogger<CosmosContext>();

            this.logger.LogTrace("Construction of Cosmos Context starting");

            // Validate input arguments
            cosmosConfiguration = Ensure.IsNotNull(() => cosmosConfiguration);
            cosmosConfiguration.Validate();

            // Set properties
            this.databaseName = Ensure.IsNotNullOrWhitespace(() => cosmosConfiguration.DatabaseId);
            this.connectionString = Ensure.IsNotNullOrWhitespace(() => cosmosConfiguration.ConnectionString);

            this.logger.LogTrace("Construction of Cosmos Context complete");
        }

        /// <summary>
        /// Gets or sets Database Users
        /// </summary>
        public DbSet<AdminUser> Users { get; set; }

        /// <summary>
        /// Configures Connection's Cosmos DB Configuration
        /// </summary>
        /// <param name="optionsBuilder">OptionsBuilder to configure EF Core</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseCosmos(this.connectionString, databaseName: this.databaseName);

        /// <summary>
        /// Configure Model Building and Mapping
        /// </summary>
        /// <param name="model">Model to build and map</param>
        protected override void OnModelCreating(ModelBuilder model)
        {
            // Configuration of Users Model
            var usersModel = model.Entity<AdminUser>();
            usersModel.ToContainer(UserBase.ContainerId);
            usersModel.HasPartitionKey(user => user.Id);
            usersModel.HasBaseType(typeof(UserBase));

            // Enum to String Converters
            usersModel
                .Property(user => user.Chapter)
                .HasConversion<string>();

            usersModel
                .Property(user => user.Level)
                .HasConversion<string>();

            usersModel
                .Property(user => user.Country)
                .HasConversion<string>();

            usersModel
                .Property(user => user.EnteredInFacebookChapter)
                .HasConversion<string>();

            usersModel
                .Property(user => user.EnteredInFacebookWki)
                .HasConversion<string>();

            usersModel
                .Property(user => user.Status)
                .HasConversion<string>();

            usersModel.OwnsMany(user => user.Positions)
                .Property(position => position.Name)
                .HasConversion<string>();
        }
    }
}
