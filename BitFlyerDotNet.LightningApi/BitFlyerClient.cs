//==============================================================================
// Copyright (c) 2017-2019 Fiats Inc. All rights reserved.
// https://www.fiats.asia/
//

using System;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using Newtonsoft.Json;

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

        public BitFlyerClientBase(string apiKey, string apiSecret)
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

        public BitFlyerResponse<T> Get<T>(string apiName, string queryParameters = "")
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

        internal BitFlyerResponse<T> PrivateGet<T>(string apiName, string queryParameters = "")
        {
            if (!IsPrivateApiEnabled)
            {
                throw new NotSupportedException("Access key and secret required.");
            }

            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ff");
            var path = _privateBasePath + apiName.ToLower();
            if (!string.IsNullOrEmpty(queryParameters))
            {
                path += "?" + queryParameters;
            }

            var text = timestamp + "GET" + path;
            var sign = BitConverter.ToString(_hmac.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", string.Empty).ToLower();
            using (var request = new HttpRequestMessage(HttpMethod.Get, path))
            {
                request.Headers.Clear();
                request.Headers.Add("ACCESS-KEY", _apiKey);
                request.Headers.Add("ACCESS-TIMESTAMP", timestamp);
                request.Headers.Add("ACCESS-SIGN", sign);

                var response = new BitFlyerResponse<T>();
                try
                {
                    response.ParseResponseMessage(text, _client.SendAsync(request).Result);
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

        internal BitFlyerResponse<T> PrivatePost<T>(string apiName, string body)
        {
            if (!IsPrivateApiEnabled)
            {
                throw new NotSupportedException("Access key and secret required.");
            }

            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ff");
            var path = _privateBasePath + apiName.ToLower();

            var text = timestamp + "POST" + path + body;
            var sign = BitConverter.ToString(_hmac.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", string.Empty).ToLower();
            using (var request = new HttpRequestMessage(HttpMethod.Post, path))
            using (var content = new StringContent(body))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                request.Content = content;
                request.Headers.Clear();
                request.Headers.Add("ACCESS-KEY", _apiKey);
                request.Headers.Add("ACCESS-TIMESTAMP", timestamp);
                request.Headers.Add("ACCESS-SIGN", sign);

                var response = new BitFlyerResponse<T>();
                try
                {
                    response.ParseResponseMessage(text, _client.SendAsync(request).Result);
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
