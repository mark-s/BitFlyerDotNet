using System;
using System.Net;
using System.Net.Http;

using BitFlyerDotNet.LightningApi.DTOs;

using Newtonsoft.Json;

namespace BitFlyerDotNet.LightningApi.Domain
{
    public class BitFlyerResponse<T> : IBitFlyerResponse
    {
        private string JsonEmpty => typeof(T) != typeof(string) && typeof(T).IsArray ? "[]" : "{}";
        
        public HttpStatusCode StatusCode { get; internal set; }
        public ErrorResponse ErrorResponse { get; internal set; } = ErrorResponse.Default;
        public bool IsUnauthorized => StatusCode == HttpStatusCode.Unauthorized;

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                if (!string.IsNullOrEmpty(_errorMessage))
                    return _errorMessage;

                if (StatusCode != HttpStatusCode.OK)
                    return StatusCode.ToString();

                return ErrorResponse != ErrorResponse.Default ? ErrorResponse.ErrorMessage : "Success";
            }
            set => _errorMessage = value;
        }

        public bool IsNetworkError => StatusCode != HttpStatusCode.OK;
        public bool IsApplicationError => ErrorResponse != ErrorResponse.Default;
        public bool IsError => StatusCode != HttpStatusCode.OK || ErrorResponse != ErrorResponse.Default;
        public bool IsEmpty => string.IsNullOrEmpty(_json) || _json == JsonEmpty;
        public bool IsErrorOrEmpty => IsError || IsEmpty;
        public Exception Exception { get; internal set; }

        private T _result;
        public T GetResult()
        {
            if (!object.Equals(_result, default(T)))
                return _result;

            _result = JsonConvert.DeserializeObject<T>(_json, JsonSerializerSettingsFactory.GetDefaultSettings());

            return _result;
        }

        private string _json;
        public string Json
        {
            get => _json;
            private set
            {
                if (value.Contains("error_message"))
                    ErrorResponse = JsonConvert.DeserializeObject<ErrorResponse>(value, JsonSerializerSettingsFactory.GetDefaultSettings());
                else
                    _json = value;
            }
        }

        public BitFlyerResponse()
        {
            _json = JsonEmpty;
        }

        internal void ParseResponseMessage(HttpResponseMessage message)
        {
            StatusCode = message.StatusCode;
            Json = message.Content.ReadAsStringAsync().Result;
        }
    }
}
