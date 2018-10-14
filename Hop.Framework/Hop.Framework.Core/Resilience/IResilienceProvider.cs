using System;
using System.Threading.Tasks;

namespace Hop.Framework.Core.Resilience
{
    public interface IResilienceProvider
    {
        TResult GetResultWithRetryProvider<TResult, TWachForException>(Func<TResult> resultFunction, Action<Exception, int> onRetry = null, int retryTimes = 3) where TWachForException : Exception;
        TResult GetResultWithRetryProvider<TResult>(Func<TResult> resultFunction, Action<Exception, int> onRetry = null, int retryTimes = 3);

        Task<TResult> GetResultWithRetryProviderAsync<TResult, TWachForException>(Func<Task<TResult>> resultFunction, Action<TWachForException, int> onRetry = null, int retryTimes = 3) where TWachForException : Exception;
        Task<TResult> GetResultWithRetryProviderAsync<TResult>(Func<Task<TResult>> resultFunction, Action<Exception, int> onRetry = null, int retryTimes = 3);
    }
}
