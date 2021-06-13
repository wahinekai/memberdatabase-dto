// -----------------------------------------------------------------------
// <copyright file="ISearchRepository.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Contract for a Search Repository
    /// </summary>
    public interface ISearchRepository
    {
        /// <summary>
        /// Perform a search, returning documents returning to the query
        /// </summary>
        /// <param name="query">The query to search with</param>
        /// <returns>An ordered list of documents returned from the query</returns>
        public Task<IList<Guid>> SearchAsync(string? query);

        /// <summary>
        /// Suggest the top five items based on the partial query
        /// </summary>
        /// <param name="partialQuery">The partially completed query to suggest on</param>
        /// <returns>The ids of each suggested document</returns>
        public Task<IList<Guid>> SuggestAsync(string partialQuery);

        /// <summary>
        /// Auto-complete the current partial query
        /// </summary>
        /// <param name="partialQuery">The partially completed query to auto-complete</param>
        /// <returns>The remaining text in the top auto-completion</returns>
        public Task<string> AutoCompleteAsync(string partialQuery);
    }
}
