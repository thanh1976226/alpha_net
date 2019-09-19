﻿using ESEIM.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ESEIM.Utils
{
    public sealed class ActionDisposable : IDisposable
    {
        private Action action;
        public ActionDisposable(Action action)
        {
            this.action = action;
        }
        public void Dispose()
        {
            var action = this.action;
            if (action != null)
            {
                action();
            }
        }
    }
    public class LazyDictionary<TKey, TValue>
    {
        private ConcurrentDictionary<TKey, Lazy<TValue>> dictionary =
            new ConcurrentDictionary<TKey, Lazy<TValue>>();
        public TValue GetOrAdd(TKey key, Func<TValue> valueGenerator)
        {
            var lazyValue = dictionary.GetOrAdd(key,
                k => new Lazy<TValue>(valueGenerator));
            return lazyValue.Value;
        }
    }
    public class AsyncLocker<T>
    {
        private LazyDictionary<T, SemaphoreSlim> semaphoreDictionary =
            new LazyDictionary<T, SemaphoreSlim>();

        public async Task<IDisposable> LockAsync(T key)
        {
            var semaphore = semaphoreDictionary.GetOrAdd(key, () => new SemaphoreSlim(1, 1));
            await semaphore.WaitAsync();
            return new ActionDisposable(() => semaphore.Release());
        }
    }
}
