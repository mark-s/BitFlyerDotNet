using System.Collections.Generic;
using System.Linq;

using BitFlyerDotNet.LightningApi.Public;

namespace BitFlyerDotNet.LightningApi.Domain
{
    internal static class ProductCodeAliases
    {
        private static readonly Dictionary<string, string> _productCodeAliases = new Dictionary<string, string>();

        public static string GetProductCodeFromAlias(string alias)
            => _productCodeAliases[alias];

        public static bool HasGotProductCodes()
            => _productCodeAliases.Any();


        public static void Populate(IEnumerable<BfMarket> markets)
        {

            foreach (var market in markets)
            {
                if (string.IsNullOrEmpty(market.Alias) == false)
                    _productCodeAliases[market.Alias] = market.ProductCode;
                else
                    _productCodeAliases[market.ProductCode] = market.ProductCode;
            }


        }
    }
}
