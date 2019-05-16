//==============================================================================
// Copyright (c) 2017-2019 Fiats Inc. All rights reserved.
// https://www.fiats.asia/
//

using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BitFlyerDotNet.LightningApi
{


    public abstract class BitFlyerClientBase : IDisposable
    {


        private const string _baseUri = "https://api.bitflyer.jp";
        private const string _publicBasePath = "/v1/";
        private const string _privateBasePath = "/v1/me/";
        protected const string _usaMarket = "/usa";
        protected const string _euMarket = "/eu";

        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly HMACSHA256 _hmac;

        public bool IsPrivateApiEnabled { get { return _hmac != null; } }

        public BitFlyerClientBase()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_baseUri);
        }

        protected BitFlyerClientBase(string apiKey, string apiSecret)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            _apiKey = apiKey;
            _hmac = new HMACSHA256(Encoding.UTF8.GetBytes(apiSecret));
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_baseUri);
        }

        public void Dispose()
        {
            _client.Dispose();
            _hmac.Dispose();
        }

        protected BitFlyerResponse<T> Get<T>(string apiName, string queryParameters = "")
        {
            var path = _publicBasePath + apiName.ToLower();
            if (!string.IsNullOrEmpty(queryParameters))
            {
                path += "?" + queryParameters;
            }

            using (var request = new HttpRequestMessage(HttpMethod.Get, path))
            {
                var response = new BitFlyerResponse<T>();
                try
                {
                    response.ParseResponseMessage(path, _client.SendAsync(request).Result);
                    return response;
                }
                catch (AggregateException aex)
                {
                    var ex = aex.InnerException;
                    response.Exception = ex;
                    if (ex is TaskCanceledException) // Caused timedout
                    {
                        response.StatusCode = HttpStatusCode.RequestTimeout;
                    }
                    else if (ex is HttpRequestException)
                    {
                        if (ex.InnerException is WebException)
                        {
                            response.ErrorMessage = ((WebException)ex.InnerException).Status.ToString();
                            response.StatusCode = HttpStatusCode.InternalServerError;
                        }
                    }
                    else if (ex is WebException)
                    {
                        var we = ex.InnerException as WebException;
                        var resp = we.Response as HttpWebResponse;
                        if (resp != null)
                        {
                            response.StatusCode = resp.StatusCode;
                        }
                        else
                        {
                            response.StatusCode = HttpStatusCode.NoContent;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                    return response;
                }
            }
        }

    }
}
