//==============================================================================
// Copyright (c) 2017-2019 Fiats Inc. All rights reserved.
// https://www.fiats.asia/
//

using System;

namespace BitFlyerDotNet.LightningApi.Domain
{
    public static class DateTimeUtil
    {
        public static DateTime Round(this DateTime dt, TimeSpan period)
        {
            return new DateTime(dt.Ticks / period.Ticks * period.Ticks, dt.Kind);
        }
    }
}
