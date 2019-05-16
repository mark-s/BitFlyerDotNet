namespace BitFlyerDotNet.LightningApi.Public
{
    public class BitFlyerClient : BitFlyerClientBase
    {
        public BitFlyerResponse<BfBoard> GetBoard(BfProductCode productCode)
            => Get<BfBoard>(nameof(GetBoard), "product_code=" + productCode.ToEnumString());

        public BitFlyerResponse<BfBoardStateResult> GetBoardState(BfProductCode productCode) 
            => Get<BfBoardStateResult>(nameof(GetBoardState), "product_code=" + productCode.ToEnumString());

        public BitFlyerResponse<BfExchangeHealth> GetExchangeHealth(BfProductCode productCode) 
            => Get<BfExchangeHealth>("gethealth", "product_code=" + productCode.ToEnumString());

        public BitFlyerResponse<BfExecution[]> GetExecutions(BfProductCode productCode, int count = 0, int before = 0, int after = 0)
        {
            var query = string.Format("product_code={0}{1}{2}{3}",
                productCode.ToEnumString(),
                (count > 0) ? $"&count={count}" : "",
                (before > 0) ? $"&before={before}" : "",
                (after > 0) ? $"&after={after}" : ""
            );
            return Get<BfExecution[]>(nameof(GetExecutions), query);
        }

        public BitFlyerResponse<BfMarket[]> GetMarkets() 
            => Get<BfMarket[]>(nameof(GetMarkets));

        public BitFlyerResponse<BfMarket[]> GetMarketsUsa() 
            => Get<BfMarket[]>(nameof(GetMarkets) + _usaMarket);

        public BitFlyerResponse<BfMarket[]> GetMarketsEu() 
            => Get<BfMarket[]>(nameof(GetMarkets) + _euMarket);

        public BitFlyerResponse<BfMarket[]>[] GetMarketsAll()
        {
            return new[]
            {
                GetMarkets(),
                GetMarketsUsa(),
                GetMarketsEu()
            };
        }

        public BitFlyerResponse<BfTicker> GetTicker(BfProductCode productCode) 
            => Get<BfTicker>(nameof(GetTicker), "product_code=" + productCode.ToEnumString());
    }
}