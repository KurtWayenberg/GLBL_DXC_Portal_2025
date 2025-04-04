using System;
using System.Threading;
using System.Threading.Tasks;

namespace DXC.Technology.Objects
{
    /// <summary>
    /// Provides helper methods for running asynchronous tasks synchronously.
    /// </summary>
    public static class AsyncHelper
    {
        #region Static Fields

        /// <summary>
        /// Task factory used to run synchronous tasks.
        /// </summary>
        private static readonly TaskFactory taskFactory = new
            TaskFactory(CancellationToken.None,
                        TaskCreationOptions.None,
                        TaskContinuationOptions.None,
                        TaskScheduler.Default);

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Runs an asynchronous function synchronously and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The asynchronous function to run.</param>
        /// <returns>The result of the asynchronous function.</returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return taskFactory
                .StartNew(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Runs an asynchronous action synchronously.
        /// </summary>
        /// <param name="func">The asynchronous action to run.</param>
        public static void RunSync(Func<Task> func)
        {
            taskFactory
                .StartNew(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
        }

        #endregion
    }
}