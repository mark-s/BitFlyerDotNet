//==============================================================================
// Copyright (c) 2017-2019 Fiats Inc. All rights reserved.
// https://www.fiats.asia/
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using BitFlyerDotNet.LightningApi.Public;
using BitFlyerDotNet.LightningApi.Realtime;
using Newtonsoft.Json.Linq;
using WebSocket4Net;
using ErrorEventArgs = SuperSocket.ClientEngine.ErrorEventArgs;

namespace BitFlyerDotNet.LightningApi
{
    public class RealtimeSourceFactory : IDisposable
    {
        public delegate void RealtimeErrorHandler(ErrorStatus error);
        public RealtimeErrorHandler ErrorHandlers;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly WebSocket _webSocket;
        private readonly AutoResetEvent _openedEvent = new AutoResetEvent(false);
        private readonly ConcurrentDictionary<string, IRealtimeSource> _webSocketSources = new ConcurrentDictionary<string, IRealtimeSource>();
        private readonly Timer _wsReconnectionTimer;
        private const int WebSocketReconnectionIntervalMs = 5000;

        private readonly BitFlyerClient _client = new BitFlyerClient();
        private readonly Dictionary<string, string> _productCodeAliases = new Dictionary<string, string>();

        private bool _opened = false;


        public RealtimeSourceFactory()
        {
            _webSocket = new WebSocket("wss://ws.lightstream.bitflyer.com/json-rpc");
            _webSocket.Security.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;

            _wsReconnectionTimer = new Timer(_ => TimerCallback());

            _openedEvent.Reset();

            _webSocket.Opened += (_,__) => OnOpened();

            _webSocket.MessageReceived += (_, args) => OnMessageReceived(args);

            _webSocket.Error += (_, eArgs) => OnError(eArgs);

            _webSocket.Closed += (_, __) =>  OnClosed();
        }


        private readonly ConcurrentDictionary<ProductCode, IObservable<BfTicker>> _tickSources = new ConcurrentDictionary<ProductCode, IObservable<BfTicker>>();
        public IObservable<BfTicker> GetTickerSource(ProductCode productCode)
        {
            TryOpen();
            return _tickSources.GetOrAdd(productCode, _ =>
            {
                InitProductCodeAliases();
                var realProductCode = _productCodeAliases[productCode.ToEnumString()];
                var source = new RealtimeTickerSource(_webSocket, JsonSerializerSettingsFactory.GetDefaultSettings(), realProductCode);
                _webSocketSources[source.Channel] = source;
                return source.Publish().RefCount();
            });
        }

        
        private void OnClosed()
        {
            Debug.WriteLine("{0} WebSocket connection closed. Will be reopening...", DateTime.Now);
            _wsReconnectionTimer.Change(WebSocketReconnectionIntervalMs, Timeout.Infinite);
        }

        private void OnError(ErrorEventArgs e)
        {
            // Classifies expected or unexpected
            var error = new ErrorStatus();
            switch (e.Exception)
            {
                case IOException ioex:
                    error.Message = (ioex.InnerException != null) ? ioex.InnerException.Message : ioex.Message;
                    break;

                case SocketException sockex:
                    Debug.WriteLine("{0} WebSocket socket error({1})", DateTime.Now, sockex.SocketErrorCode);
                    Debug.WriteLine("{0} WebSocket caused exception. Will be closed.", DateTime.Now, sockex.Message);
                    error.SocketError = sockex.SocketErrorCode;
                    error.Message = sockex.Message;
                    break;

                default:
                    switch ((uint) e.Exception.HResult)
                    {
                        case 0x80131500: // Bad gateway - probably terminated from host
                            error.Message = e.Exception.Message;
                            break;

                        default: // Unexpected exception
                            throw e.Exception;
                    }

                    break;
            }

            ErrorHandlers?.Invoke(error);
        }

        private void OnMessageReceived(MessageReceivedEventArgs args)
        {
            var subscriptionResult = JObject.Parse(args.Message)["params"];
            var channel = subscriptionResult["channel"].Value<string>();
            _webSocketSources[channel].OnSubscribe(subscriptionResult["message"]);
        }

        private void OnOpened()
        {
            Debug.WriteLine("{0} WebSocket opened.", DateTime.Now);

            _wsReconnectionTimer.Change(Timeout.Infinite, Timeout.Infinite); // stop

            if (_webSocketSources.Any())
            {
                Debug.WriteLine("{0} WebSocket recover subscriptions.", DateTime.Now);
                _webSocketSources.Values.ForEach(source => { source.Subscribe(); }); // resubscribe
            }

            _openedEvent.Set();
        }

        private void TimerCallback()
        {
            _wsReconnectionTimer.Change(Timeout.Infinite, Timeout.Infinite); // stop

            Debug.WriteLine("{0} WebSocket is reopening connection... state={1}", DateTime.Now, _webSocket.State);

            switch (_webSocket.State)
            {
                case WebSocketState.None:
                case WebSocketState.Closed:
                    try
                    {
                        _webSocket.Open();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        _wsReconnectionTimer.Change(WebSocketReconnectionIntervalMs, Timeout.Infinite); // restart
                    }

                    break;

                case WebSocketState.Open:
                    Debug.WriteLine("{0} Web socket is still opened.", DateTime.Now);
                    break;

                default:
                    _wsReconnectionTimer.Change(WebSocketReconnectionIntervalMs, Timeout.Infinite); // restart
                    break;
            }
        }
        
        private void TryOpen()
        {
            if (_opened) return;

            _opened = true;
            _webSocket.Open();
            _openedEvent.WaitOne(TimeSpan.FromSeconds(10));
        }

        private void InitProductCodeAliases()
        {
            if (_productCodeAliases.Any())
                return; // Already initialized

            
            var allMarketsResponse = _client.GetAllMarkets();
            foreach (var response in allMarketsResponse)
            {
                if (response.IsError)
                {
                    if (response.Exception != null)
                        throw response.Exception;
                    else
                        throw new InvalidOperationException(response.ErrorMessage);
                }

                foreach (var market in response.GetResult())
                    ConvertAliasToProductCode(market);
            }
        }

        private void ConvertAliasToProductCode(BfMarket market)
        {
            if (string.IsNullOrEmpty(market.Alias) == false)
                _productCodeAliases[market.Alias] = market.ProductCode;
            else
                _productCodeAliases[market.ProductCode] = market.ProductCode;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        
    }
}
