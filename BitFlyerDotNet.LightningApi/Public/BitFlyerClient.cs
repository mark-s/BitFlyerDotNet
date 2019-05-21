using System.Threading.Tasks;

using BitFlyerDotNet.LightningApi.Domain;

namespace BitFlyerDotNet.LightningApi.Public
{
    public class BitFlyerClient : BitFlyerClientBase
    {

        public async Task<BitFlyerResponse<BfBoard>> GetBoardAsync(ProductCode productCode)
            => await GetAsync<BfBoard>(ApiName.GetBoard, "product_code=" + productCode.ToEnumString());

        public async Task<BitFlyerResponse<BfBoardStateResult>> GetBoardStateAsync(ProductCode productCode)
            => await GetAsync<BfBoardStateResult>(ApiName.GetBoardState, "product_code=" + productCode.ToEnumString());

        public async Task<BitFlyerResponse<BfExchangeHealth>> GetExchangeHealthAsync(ProductCode productCode)
            => await GetAsync<BfExchangeHealth>(ApiName.GetExchangeHealth, "product_code=" + productCode.ToEnumString());

        public async Task<BitFlyerResponse<BfMarket[]>> GetMarketsJpAsync()
            => await GetAsync<BfMarket[]>(ApiName.GetMarketsJP);

        public async Task<BitFlyerResponse<BfMarket[]>> GetMarketsUsaAsync()
            => await GetAsync<BfMarket[]>(ApiName.GetMarketsUSA);

        public async Task<BitFlyerResponse<BfMarket[]>> GetMarketsEuAsync()
            => await GetAsync<BfMarket[]>(ApiName.GetMarketsEU);

        public async Task<BitFlyerResponse<BfTicker>> GetTicker(ProductCode productCode)
            => await GetAsync<BfTicker>(ApiName.GetTicker, "product_code=" + productCode.ToEnumString());

        public async Task<BitFlyerResponse<BfMarket[]>[]> GetAllMarketsAsync()
        {
            var jpAsync = GetMarketsJpAsync();
            var usaAsync = GetMarketsUsaAsync();
            var euAsync = GetMarketsEuAsync();

            await Task.WhenAll(jpAsync, usaAsync, euAsync);

            return new[]
            {
                jpAsync.Result,
                usaAsync.Result,
                euAsync.Result,
            };

        }

        

    }
}
