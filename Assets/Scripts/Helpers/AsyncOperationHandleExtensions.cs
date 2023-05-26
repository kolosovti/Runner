using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.Helpers
{
    public static class AsyncOperationHandleExtensions
    {
        public static void AddTo<T>(this AsyncOperationHandle<T> handle, List<AsyncOperationHandle> collection)
        {
            collection.Add(handle);
        }
    }
}