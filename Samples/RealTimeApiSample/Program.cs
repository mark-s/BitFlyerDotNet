using System;
using BitFlyerDotNet.LightningApi;

namespace RealTimeApiSample
{
    internal class Program
    {
        private static char GetCh(bool echo = true) { var ch = Char.ToUpper(Console.ReadKey(true).KeyChar); if (echo) Console.WriteLine(ch); return ch; }

        private static void Main(string[] args)
        {
            var factory = new RealtimeSourceFactory();
            factory.ErrorHandlers += (error) =>
            {
                Console.WriteLine("Error: {0} Socket Error = {1}", error.Message, error.SocketError);
            };

            Console.WriteLine("1) RealtimeExecutionSample");
            Console.WriteLine("2) RealtimeTickerSample");

            switch (GetCh())
            {
                case '1':
                    RealtimeExecutionSample(factory);
                    break;

                case '2':
                    RealtimeTickerSample(factory);
                    break;
            }

            Console.ReadLine();
        }

        private static void RealtimeExecutionSample(RealtimeSourceFactory factory)
        {
            factory.GetExecutionSource(BfProductCode.BTCJPY).Subscribe(tick =>
            {
                Console.WriteLine(BfProductCode.BTCJPY + " {0} {1} {2} {3} {4} {5}",
                    tick.ExecutionId,
                    tick.Side,
                    tick.Price,
                    tick.Size,
                    tick.ExecutedTime.ToLocalTime(),
                    tick.ChildOrderAcceptanceId);
            });
        }

        private static void RealtimeTickerSample(RealtimeSourceFactory factory)
        {
            factory.GetTickerSource(BfProductCode.BTCJPY).Subscribe(ticker =>
            {
                Console.WriteLine($"{ticker.ProductCode} {ticker.Timestamp.ToLocalTime()} {ticker.BestBid} {ticker.BestAsk} {ticker.LastTradedPrice}");
            });
        }
    }
}
