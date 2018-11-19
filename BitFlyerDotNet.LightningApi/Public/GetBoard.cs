﻿//==============================================================================
// Copyright (c) 2017-2018 Fiats Inc. All rights reserved.
// http://www.fiats.asia/
//

using Newtonsoft.Json;
using Fiats.Utils;

namespace BitFlyerDotNet.LightningApi
{
    public class BfBoardOrder
    {
        [JsonProperty(PropertyName = "price")]
        public double Price { get; private set; }

        [JsonProperty(PropertyName = "size")]
        public double Size { get; private set; }
    }

    public class BfBoard
    {
        [JsonProperty(PropertyName = "mid_price")]
        public double MidPrice { get; private set; }

        [JsonProperty(PropertyName = "bids")]
        public BfBoardOrder[] Bids { get; private set; }

        [JsonProperty(PropertyName = "asks")]
        public BfBoardOrder[] Asks { get; private set; }
    }

    public partial class BitFlyerClient
    {
        public BitFlyerResponse<BfBoard> GetBoard(BfProductCode productCode)
        {
            return Get<BfBoard>(nameof(GetBoard), "product_code=" + productCode.ToEnumString());
        }
    }
}
