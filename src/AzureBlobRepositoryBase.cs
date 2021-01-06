// -----------------------------------------------------------------------
// <copyright file="AzureBlobRepositoryBase.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto
{
    using Azure.Storage.Blobs;
    using Microsoft.Extensions.Logging;
    using WahineKai.Common;
    using WahineKai.MemberDatabase.Dto.Properties;

    /// <summary>
    /// Base class for all respositories related to Azure Blob Storage
    /// </summary>
    public abstract class AzureBlobRepositoryBase : RepositoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureBlobRepositoryBase"/> class.
        /// </summary>
        /// <param name="configuration">Azure Blob Storage Configuration for this application.</param>
        /// <param name="containerName">Container name that this repository uses</param>
        /// <param name="loggerFactory">Logger factory for logging</param>
        public AzureBlobRepositoryBase(AzureBlobConfiguration configuration, string containerName, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.Logger.LogTrace("Beginning construction of Azure Blob Repository Base");

            // Sanity check input arguments
            configuration = Ensure.IsNotNull(() => configuration);
            configuration.Validate();

            this.BlobContainerClient = new BlobContainerClient(configuration.ConnectionString, containerName);

            this.Logger.LogTrace("Construction of Azure Blob Repository Base Complete");
        }

        /// <summary>
        /// Gets container client for this blob repository
        /// </summary>
        protected BlobContainerClient BlobContainerClient { get; init; }
    }
}
