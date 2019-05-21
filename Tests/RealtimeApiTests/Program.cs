//==============================================================================
// Copyright (c) 2017-2019 Fiats Inc. All rights reserved.
// https://www.fiats.asia/
//

using System;

using BitFlyerDotNet.LightningApi;

namespace RealtimeApiTests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var factory = new RealtimeSourceFactory();
            //var disp = SubscribeTickerSource(factory, ProductCode.BTCUSD);
            var disp = SubscribeExecutionSource(factory, ProductCode.FXBTCJPY);
            Console.ReadLine();
            disp.Dispose();
        }


        private static IDisposable SubscribeExecutionSource(RealtimeSourceFactory factory, ProductCode productCode)
        {
            var source = factory.GetExecutionSource(productCode);
            return source.Subscribe(exec =>
            {
                Console.WriteLine($"{exec.ExecutedTime} P:{exec.Price} A:{exec.Side} B:{exec.Size}");
            });

        }
    }
}
