namespace BitFlyerDotNet.LightningApi.Public
{
    public class BitFlyerClient : BitFlyerClientBase
    {

        public BitFlyerResponse<BfBoard> GetBoard(ProductCode productCode)
            => Get<BfBoard>(ApiName.GetBoard, "product_code=" + productCode.ToEnumString());

        public BitFlyerResponse<BfBoardStateResult> GetBoardState(ProductCode productCode)
            => Get<BfBoardStateResult>(ApiName.GetBoardState, "product_code=" + productCode.ToEnumString());

        public BitFlyerResponse<BfExchangeHealth> GetExchangeHealth(ProductCode productCode)
            => Get<BfExchangeHealth>(ApiName.GetExchangeHealth, "product_code=" + productCode.ToEnumString());

        public BitFlyerResponse<BfExecution[]> GetExecutions(ProductCode productCode, int count = 0, int before = 0, int after = 0)
        {
            var query = string.Format("product_code={0}{1}{2}{3}",
                productCode.ToEnumString(),
                (count > 0) ? $"&count={count}" : "",
                (before > 0) ? $"&before={before}" : "",
                (after > 0) ? $"&after={after}" : ""
            );
            return Get<BfExecution[]>(ApiName.GetExecutions, query);
        }

        public BitFlyerResponse<BfMarket[]> GetMarketsJp()
            => Get<BfMarket[]>(ApiName.GetMarketsJP);

        public BitFlyerResponse<BfMarket[]> GetMarketsUsa()
            => Get<BfMarket[]>(ApiName.GetMarketsUSA);

        public BitFlyerResponse<BfMarket[]> GetMarketsEu()
            => Get<BfMarket[]>(ApiName.GetMarketsEU);

        public BitFlyerResponse<BfMarket[]>[] GetAllMarkets()
        {
            return new[]
            {
                GetMarketsJp(),
                GetMarketsUsa(),
                GetMarketsEu()
            };
        }

        public BitFlyerResponse<BfTicker> GetTicker(ProductCode productCode)
            => Get<BfTicker>(ApiName.GetTicker, "product_code=" + productCode.ToEnumString());
    }
}