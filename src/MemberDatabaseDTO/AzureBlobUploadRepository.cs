// -----------------------------------------------------------------------
// <copyright file="AzureBlobUploadRepository.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Azure.Storage.Blobs.Models;
    using Microsoft.Extensions.Logging;
    using WahineKai.Common;
    using WahineKai.MemberDatabase.Dto.Contracts;
    using WahineKai.MemberDatabase.Dto.Properties;

    /// <summary>
    /// Repository to upload profile picutres
    /// </summary>
    public class AzureBlobUploadRepository : AzureBlobRepositoryBase, IUploadRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureBlobUploadRepository"/> class.
        /// </summary>
        /// <param name="configuration">Azure Blob Storage Configuration for this application.</param>
        /// <param name="containerName">Container name that this repository uses</param>
        /// <param name="loggerFactory">Logger factory for logging</param>
        public AzureBlobUploadRepository(AzureBlobConfiguration configuration, string containerName, ILoggerFactory loggerFactory)
            : base(configuration, containerName, loggerFactory)
        {
        }

        /// <inheritdoc/>
        public async Task<string> UploadAsync(string fileName, Stream pictureStream)
        {
            // Sanity check input
            fileName = Ensure.IsNotNullOrWhitespace(() => fileName);
            pictureStream = Ensure.IsNotNull(() => pictureStream);

            var blobClient = this.BlobContainerClient.GetBlobClient(fileName);
            var blobResponse = await this.WithRetriesAsync<BlobContentInfo, Exception>(
                async () => await blobClient.UploadAsync(pictureStream, overwrite: true));
            return blobClient.Uri.AbsoluteUri;
        }
    }
}
