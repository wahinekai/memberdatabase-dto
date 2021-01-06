// -----------------------------------------------------------------------
// <copyright file="IAzureActiveDirectoryRepository.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Contracts
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for interactions with Azure Active Directory Repository
    /// </summary>
    public interface IAzureActiveDirectoryRepository
    {
        /// <summary>
        /// Updates the email of a user in Azure Active Directory
        /// </summary>
        /// <param name="oldEmail">The user's old email</param>
        /// <param name="newEmail">The email to update it to</param>
        /// <returns>A <see cref="Task"/></returns>
        public Task UpdateUserEmailAsync(string oldEmail, string newEmail);
    }
}
