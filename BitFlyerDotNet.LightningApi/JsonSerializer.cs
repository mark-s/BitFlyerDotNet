using System;
using Newtonsoft.Json;

namespace BitFlyerDotNet.LightningApi
{
    internal static class JsonSerializerSettingsFactory
    {
        private static readonly Lazy<JsonSerializerSettings> _lazyDefaultSettings = new Lazy<JsonSerializerSettings>(() => new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        });

        public static JsonSerializerSettings GetDefaultSettings()
            => _lazyDefaultSettings.Value;
    }
}