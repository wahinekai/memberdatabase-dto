// -----------------------------------------------------------------------
// <copyright file="RepositoryBase.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Polly;
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

        /// <summary>
        /// Execute an action with retries asynchronously
        /// </summary>
        /// <typeparam name="TExceptionType">The type of exception to catch</typeparam>
        /// <param name="action">The action to execute</param>
        /// <param name="maxRetries">The maximum number of retries to execute before failing.</param>
        /// <param name="retryDelayInSeconds">The amount of time to wait between retries.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        protected async Task WithRetriesAsync<TExceptionType>(Action action, int maxRetries = 10, int retryDelayInSeconds = 1)
            where TExceptionType : Exception
        {
            await Policy.Handle<TExceptionType>()
                .WaitAndRetryAsync(maxRetries, retryAttempt => TimeSpan.FromSeconds(retryDelayInSeconds))
                .ExecuteAsync(async () => await Task.Run(action));
        }

        /// <summary>
        /// Execute an action with retries asynchronously
        /// </summary>
        /// <typeparam name="TExceptionType">The type of exception to catch</typeparam>
        /// <param name="action">The action to execute</param>
        /// <param name="maxRetries">The maximum number of retries to execute before failing.</param>
        /// <param name="retryDelayInSeconds">The amount of time to wait between retries.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        protected async Task WithRetriesAsync<TExceptionType>(Func<Task> action, int maxRetries = 10, int retryDelayInSeconds = 1)
            where TExceptionType : Exception
        {
            await Policy.Handle<TExceptionType>()
                .WaitAndRetryAsync(maxRetries, retryAttempt => TimeSpan.FromSeconds(retryDelayInSeconds))
                .ExecuteAsync(async () => await action());
        }

        /// <summary>
        /// Execute an action with retries asynchronously, returning the result
        /// </summary>
        /// <typeparam name="TReturnType">The function's return type</typeparam>
        /// <typeparam name="TExceptionType">The type of exception to catch</typeparam>
        /// <param name="action">The action to execute</param>
        /// <param name="maxRetries">The maximum number of retries to execute before failing.</param>
        /// <param name="retryDelayInSeconds">The amount of time to wait between retries.</param>
        /// <returns>The return from the function.</returns>
        protected async Task<TReturnType> WithRetriesAsync<TReturnType, TExceptionType>(Func<TReturnType> action, int maxRetries = 10, int retryDelayInSeconds = 1)
            where TExceptionType : Exception
        {
            return await Policy.Handle<TExceptionType>()
                .WaitAndRetryAsync(maxRetries, retryAttempt => TimeSpan.FromSeconds(retryDelayInSeconds))
                .ExecuteAsync(async () => await Task.Run(action));
        }

        /// <summary>
        /// Execute an action with retries asynchronously, returning the result
        /// </summary>
        /// <typeparam name="TReturnType">The function's return type</typeparam>
        /// <typeparam name="TExceptionType">The type of exception to catch</typeparam>
        /// <param name="action">The action to execute</param>
        /// <param name="maxRetries">The maximum number of retries to execute before failing.</param>
        /// <param name="retryDelayInSeconds">The amount of time to wait between retries.</param>
        /// <returns>The return from the function.</returns>
        protected async Task<TReturnType> WithRetriesAsync<TReturnType, TExceptionType>(Func<Task<TReturnType>> action, int maxRetries = 10, int retryDelayInSeconds = 1)
            where TExceptionType : Exception
        {
            return await Policy.Handle<TExceptionType>()
                .WaitAndRetryAsync(maxRetries, retryAttempt => TimeSpan.FromSeconds(retryDelayInSeconds))
                .ExecuteAsync(async () => await action());
        }
    }
}
