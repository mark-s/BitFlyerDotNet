using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace BitFlyerDotNet.LightningApi
{
    public class BitFlyerResponse<T> : IBitFlyerResponse
    {
        private string JsonEmpty
        {
            get
            {
                if (typeof(T) != typeof(string) && typeof(T).IsArray)
                {
                    return "[]";
                }
                else
                {
                    return "{}";
                }
            }
        }

        private  readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };

        internal void ParseResponseMessage(string request, HttpResponseMessage message)
        {
            StatusCode = message.StatusCode;
            Json = message.Content.ReadAsStringAsync().Result;
        }

        public HttpStatusCode StatusCode { get; internal set; }
        public BfErrorResponse ErrorResponse { get; internal set; } = BfErrorResponse.Default;
        public bool IsUnauthorized { get { return StatusCode == HttpStatusCode.Unauthorized; } }

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                if (!string.IsNullOrEmpty(_errorMessage))
                {
                    return _errorMessage;
                }
                else if (StatusCode != HttpStatusCode.OK)
                {
                    return StatusCode.ToString();
                }
                else
                {
                    return ErrorResponse != BfErrorResponse.Default ? ErrorResponse.ErrorMessage : "Success";
                }
            }
            set { _errorMessage = value; }
        }

        public bool IsNetworkError { get { return StatusCode != HttpStatusCode.OK; } }
        public bool IsApplicationError { get { return ErrorResponse != BfErrorResponse.Default; } }
        public bool IsError { get { return StatusCode != HttpStatusCode.OK || ErrorResponse != BfErrorResponse.Default; } }
        public bool IsEmpty { get { return string.IsNullOrEmpty(_json) || _json == JsonEmpty; } }
        public bool IsErrorOrEmpty { get { return IsError || IsEmpty; } }
        public Exception Exception { get; internal set; }

        private T _result = default(T);
        public T GetResult()
        {
            if (object.Equals(_result, default(T)))
            {
                _result = JsonConvert.DeserializeObject<T>(_json, _jsonSettings);
            }
            return _result;
        }

        private string _json;
        public string Json
        {
            get { return _json; }
            set
            {
                if (value.Contains("error_message"))
                {
                    ErrorResponse = JsonConvert.DeserializeObject<BfErrorResponse>(value, _jsonSettings);
                }
                else
                {
                    _json = value;
                }
            }
        }

        public BitFlyerResponse()
        {
            _json = JsonEmpty;
        }
    }
}