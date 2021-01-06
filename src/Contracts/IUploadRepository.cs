// -----------------------------------------------------------------------
// <copyright file="IUploadRepository.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Contracts
{
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Contract for a profile picture repository
    /// </summary>
    public interface IUploadRepository
    {
        /// <summary>
        /// Uploads a file stream to the repository, and returns a URL to it
        /// </summary>
        /// <param name="fileName">The file name of the picture to upload</param>
        /// <param name="pictureStream">The file stream to upload</param>
        /// <returns>A URL to the picture</returns>
        public Task<string> UploadAsync(string fileName, Stream pictureStream);
    }
}
