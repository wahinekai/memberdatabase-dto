// -----------------------------------------------------------------------
// <copyright file="AzureActiveDirectoryRepository.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Graph;
    using Microsoft.Graph.Auth;
    using Microsoft.Identity.Client;
    using WahineKai.Common;
    using WahineKai.MemberDatabase.Dto.Contracts;
    using WahineKai.MemberDatabase.Dto.Properties;

    /// <summary>
    /// Implementation of Azure Active Directory Repository
    /// </summary>
    public class AzureActiveDirectoryRepository : RepositoryBase, IAzureActiveDirectoryRepository
    {
        private readonly GraphServiceClient graphServiceClient;
        private readonly string domain;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureActiveDirectoryRepository"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory for creating logger</param>
        /// <param name="configuration">Configuration for this Azure Active Directory repository</param>
        public AzureActiveDirectoryRepository(ILoggerFactory loggerFactory, AzureActiveDirectoryConfiguration configuration)
            : base(loggerFactory)
        {
            this.Logger.LogTrace("Beginning construction of Azure Active Directory Repository");

            // Sanity check input arguments
            configuration = Ensure.IsNotNull(() => configuration);
            configuration.Validate();

            // Copy over domain from configuration
            this.domain = Ensure.IsNotNullOrWhitespace(() => configuration.Domain);

            // Build Graph Service Client
            var confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(configuration.AppId)
                .WithTenantId(configuration.TenantId)
                .WithClientSecret(configuration.ClientSecret)
                .Build();

            var authProvider = new ClientCredentialProvider(confidentialClientApplication);
            this.graphServiceClient = new GraphServiceClient(authProvider);

            this.Logger.LogTrace("Completed construction of Azure Active Directory Repository");
        }

        /// <inheritdoc/>
        public async Task UpdateUserEmailAsync(string oldEmail, string newEmail)
        {
            // Sanity check input arguments
            newEmail = Ensure.IsNotNullOrWhitespace(() => newEmail);

            // Old email is user principal name
            var user = await this.GetUserByIssuedIdIfExists(oldEmail);

            if (user != null)
            {
                // Update email address sign in identity
                foreach (var identity in user.Identities)
                {
                    if (identity.SignInType.StartsWith("emailAddress"))
                    {
                        identity.IssuerAssignedId = newEmail;
                    }
                }

                this.Logger.LogDebug($"Updated email to {newEmail}");

                // Update in Microsoft Graph
                await this.graphServiceClient.Users[user.Id]
                    .Request()
                    .UpdateAsync(user);
            }
            else
            {
                this.Logger.LogDebug("No user found in AAD");
            }
        }

        private async Task<User?> GetUserByIssuedIdIfExists(string issuedId)
        {
            // Sanity check input arguments
            issuedId = Ensure.IsNotNullOrWhitespace(() => issuedId);

            this.Logger.LogInformation($"Getting user from AAD with Issued Id {issuedId}");

            // Create filter string
            var filterString = $"Identities/any(id:id/Issuer eq '{this.domain}' and id/IssuerAssignedId eq '{issuedId}')";

            // Perform query
            var queryResponse = await this.graphServiceClient.Users
                .Request()
                .Filter(filterString)
                .Select(user => new
                {
                    user.Id,
                    user.Identities,
                })
                .GetAsync();

            queryResponse = Ensure.IsNotNull(() => queryResponse);

            // If user exists
            if (queryResponse.Count == 1)
            {
                // Query response should have only one User
                Ensure.IsNotNullOrEmpty(() => queryResponse, "queryResponse");

                var user = queryResponse[0];

                // Sanity check output
                user = Ensure.IsNotNull(() => user, "user");
                Ensure.IsNotNullOrEmpty(() => user.Identities, "user.Identities");

                return user;
            }

            return null;
        }
    }
}
