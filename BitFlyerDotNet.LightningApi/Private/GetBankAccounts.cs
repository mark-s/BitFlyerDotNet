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

namespace BitFlyerDotNet.LightningApi
{
    public class BfBankAccount
    {
        [JsonProperty(PropertyName = "id")]
        public int AccountId { get; private set; }

        [JsonProperty(PropertyName = "is_verified")]
        public bool IsVerified { get; private set; }

        [JsonProperty(PropertyName = "bank_name")]
        public string BankName { get; private set; }

        [JsonProperty(PropertyName = "branch_name")]
        public string BranchName { get; private set; }

        [JsonProperty(PropertyName = "account_type")]
        public string AccountType { get; private set; }

        [JsonProperty(PropertyName = "account_number")]
        public string AccountNumber { get; private set; }

        [JsonProperty(PropertyName = "account_name")]
        public string AccountName { get; private set; }
    }

    public partial class BitFlyerClient
    {
        public BitFlyerResponse<BfBankAccount[]> GetBankAccounts()
        {
            return PrivateGet<BfBankAccount[]>(nameof(GetBankAccounts));
        }
    }
}
