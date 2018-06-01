﻿//==============================================================================
// Copyright (c) 2017-2018 Fiats Inc. All rights reserved.
// http://www.fiats.asia/
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Fiats.Utils;

namespace BitFlyerDotNet.LightningApi
{
    // Common interface between BfExecution and BfPrivateExecution
    public interface IBfExecution
    {
        int ExecutionId { get; }
        BfTradeSide Side { get; }
        double Price { get; }
        double Size { get; }
        DateTime ExecutedTime { get; }
        string ChildOrderAcceptanceId { get; }
    }

    public class BfExecution : IBfExecution
    {
        [JsonProperty(PropertyName = "id")]
        public int ExecutionId { get; private set; }

        [JsonProperty(PropertyName = "side")]
        [JsonConverter(typeof(StringEnumConverter))]
        public BfTradeSide Side { get; private set; }

        [JsonProperty(PropertyName = "price")]
        public double Price { get; private set; }

        [JsonProperty(PropertyName = "size")]
        public double Size { get; private set; }

        [JsonProperty(PropertyName = "exec_date")]
        public DateTime ExecutedTime { get; private set; }

        [JsonProperty(PropertyName = "buy_child_order_acceptance_id")]
        public string BuyChildOrderAcceptanceId { get; private set; }

        [JsonProperty(PropertyName = "sell_child_order_acceptance_id")]
        public string SellChildOrderAcceptanceId { get; private set; }

        public string ChildOrderAcceptanceId { get { return Side == BfTradeSide.Buy ? BuyChildOrderAcceptanceId : SellChildOrderAcceptanceId; } }
    }

    public partial class BitFlyerClient
    {
        public BitFlyerResponse<BfExecution[]> GetExecutions(string productCode, int count, int before, int after)
        {
            var query = string.Format("product_code={0}{1}{2}{3}",
                productCode,
                (count > 0) ? string.Format("&count={0}", count) : "",
                (before > 0) ? string.Format("&before={0}", before) : "",
                (after > 0) ? string.Format("&after={0}", after) : ""
            );
            return Get<BfExecution[]>(nameof(GetExecutions), query);
        }

        public BitFlyerResponse<BfExecution[]> GetExecutions(BfProductCode productCode, int count, int before, int after)
        {
            return GetExecutions(productCode.ToEnumString(), count, before, after);
        }
    }
}
