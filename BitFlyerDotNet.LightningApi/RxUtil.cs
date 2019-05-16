using System;
using System.Reactive.Disposables;

namespace BitFlyerDotNet.LightningApi
{
    public static class RxUtil
    {
        public static TResult AddTo<TResult>(this TResult resource, CompositeDisposable disposable) where TResult : IDisposable
        {
            disposable.Add(resource);
            return resource;
        }
    }
}