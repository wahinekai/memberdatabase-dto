// -----------------------------------------------------------------------
// <copyright file="AzureActiveDirectoryConfiguration.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Properties
{
    using Microsoft.Extensions.Configuration;
    using WahineKai.Common;
    using WahineKai.Common.Contracts;

    /// <summary>
    /// Configuration for connecting to Azure Active Directory
    /// </summary>
    public class AzureActiveDirectoryConfiguration : IValidatable
    {
        /// <summary>
        /// Gets or sets the this App's Id
        /// </summary>
        public string? AppId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Azure Active Directory Tenant
        /// </summary>
        public string? TenantId { get; set; }

        /// <summary>
        /// Gets or sets the secret of this client
        /// </summary>
        public string? ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the Azure AD Domain
        /// </summary>
        public string? Domain { get; set; }

        /// <summary>
        /// Builds an Azure Active Directory configuration from a configuration
        /// </summary>
        /// <param name="configuration">Dotnet configuration object</param>
        /// <returns>A validated Azure Blob configuration. Throws if not possible.</returns>
        public static AzureActiveDirectoryConfiguration BuildFromConfiguration(IConfiguration configuration)
        {
            var azureActiveDirectoryConfiguration = new AzureActiveDirectoryConfiguration()
            {
                AppId = configuration["AzureAd:AppId"],
                TenantId = configuration["AzureAd:TenantId"],
                ClientSecret = configuration["AzureAd:ClientSecret"],
                Domain = configuration["AzureAd:Domain"],
            };

            azureActiveDirectoryConfiguration.Validate();

            return azureActiveDirectoryConfiguration;
        }

        /// <inheritdoc/>
        public void Validate()
        {
            this.AppId = Ensure.IsNotNullOrWhitespace(() => this.AppId);
            this.TenantId = Ensure.IsNotNullOrWhitespace(() => this.TenantId);
            this.ClientSecret = Ensure.IsNotNullOrWhitespace(() => this.ClientSecret);
            this.Domain = Ensure.IsNotNullOrWhitespace(() => this.Domain);
        }
    }
}
