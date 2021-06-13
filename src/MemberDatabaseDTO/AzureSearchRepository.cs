// -----------------------------------------------------------------------
// <copyright file="AzureSearchRepository.cs" company="Wahine Kai">
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
    using Azure.Search.Documents;
    using Microsoft.Extensions.Logging;
    using WahineKai.Common;
    using WahineKai.MemberDatabase.Dto.Contracts;
    using WahineKai.MemberDatabase.Dto.Models;
    using WahineKai.MemberDatabase.Dto.Properties;

    /// <summary>
    /// Repository for Searching with Azure Search
    /// </summary>
    public class AzureSearchRepository : RepositoryBase, ISearchRepository
    {
        private readonly string? suggesterName;

        private SearchClient searchClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureSearchRepository"/> class.
        /// </summary>
        /// <param name="configuration">Azure Blob Storage Configuration for this application.</param>
        /// <param name="loggerFactory">Logger factory for logging</param>
        public AzureSearchRepository(AzureSearchConfiguration configuration, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.Logger.LogTrace("Beginning construction of Azure Blob Repository Base");

            // Sanity check input arguments
            configuration = Ensure.IsNotNull(() => configuration);
            configuration.Validate();

            this.searchClient = new SearchClient(configuration.Endpoint, configuration.IndexName, configuration.AzureKeyCredential);

            this.CanSuggest = configuration.CanSuggest ?? false;
            this.suggesterName = configuration.SuggesterName;

            this.Logger.LogTrace("Construction of Azure Blob Repository Base Complete");
        }

        /// <summary>
        /// Gets a value indicating whether this Search Repository can perform suggestions
        /// </summary>
        public bool CanSuggest { get; init; }

        /// <inheritdoc/>
        public async Task<IList<Guid>> SearchAsync(string? query)
        {
            // Sanity check input
            var queryNotNull = query ?? "*";

            // Get result of search
            var searchResponse = await this.searchClient.SearchAsync<ModelBase>(queryNotNull);

            // Cast async & paginated result to list
            var resultsList = await searchResponse.Value.GetResultsAsync().ToListAsync();

            // Get document itself - remove SearchResult wrapper
            return resultsList.Select(searchResult => searchResult.Document.Id).ToList();
        }

        /// <inheritdoc/>
        public async Task<IList<Guid>> SuggestAsync(string partialQuery)
        {
            // Ensure this repository can suggest
            Ensure.IsTrue(() => this.CanSuggest);

            // Get Suggestion Response
            var suggestResponse = await this.searchClient.SuggestAsync<ModelBase>(partialQuery, this.suggesterName);

            // Remove Azure wrappers & return as a list
            return suggestResponse.Value.Results.Select(result => result.Document.Id).ToList();
        }

        /// <inheritdoc/>
        public async Task<string> AutoCompleteAsync(string partialQuery)
        {
            // Ensure this repository can auto-complete
            Ensure.IsTrue(() => this.CanSuggest);

            // Get Auto Complete Response
            var autoCompleteResponse = await this.searchClient.AutocompleteAsync(partialQuery, this.suggesterName);

            // Get and return first auto-complete term
            var autocompleteItem = autoCompleteResponse.Value.Results.First();
            return autocompleteItem.Text;
        }
    }
}
