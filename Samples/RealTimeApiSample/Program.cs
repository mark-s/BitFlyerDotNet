using System;
using BitFlyerDotNet.LightningApi;

namespace RealTimeApiSample
{
    internal class Program
    {

        private static void Main(string[] args)
        {
            var factory = new RealtimeSourceFactory();
            factory.ErrorHandlers += (error) =>
            {
                Console.WriteLine("Error: {0} Socket Error = {1}", error.Message, error.SocketError);
            };


            RealtimeTickerSample(factory);


            Console.ReadLine();
        }



        private static void RealtimeTickerSample(RealtimeSourceFactory factory)
        {
            factory.GetTickerSource(ProductCode.BTCJPY).Subscribe(ticker =>
            {
                Console.WriteLine($"{ticker.ProductCode} {ticker.Timestamp.ToLocalTime()} {ticker.BestBid} {ticker.BestAsk} {ticker.LastTradedPrice}");
            });
        }
    }
}
