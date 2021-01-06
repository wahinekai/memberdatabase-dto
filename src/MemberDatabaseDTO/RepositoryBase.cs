// -----------------------------------------------------------------------
// <copyright file="RepositoryBase.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto
{
    using Microsoft.Extensions.Logging;
    using WahineKai.Common;

    /// <summary>
    /// Base class for all interaction with an outside data source
    /// </summary>
    public abstract class RepositoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory given by ASP.NET for generating loggers</param>
        public RepositoryBase(ILoggerFactory loggerFactory)
        {
            loggerFactory = Ensure.IsNotNull(() => loggerFactory);
            this.Logger = loggerFactory.CreateLogger<RepositoryBase>();

            this.Logger.LogTrace("Construction of repository base complete");
        }

        /// <summary>
        /// Gets logger for logging all transactions
        /// </summary>
        protected ILogger Logger { get; }
    }
}
