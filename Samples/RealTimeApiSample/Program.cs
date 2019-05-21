using System;
using System.Threading.Tasks;

using BitFlyerDotNet.LightningApi;
using BitFlyerDotNet.LightningApi.Domain;

namespace RealTimeApiSample
{
    internal class Program
    {

        private static async Task Main(string[] args)
        {
            var factory = new RealtimeSourceFactory();
            factory.ErrorHandlers += (error) =>
            {
                Console.WriteLine("Error: {0} Socket Error = {1}", error.Message, error.SocketError);
            };


             await RealtimeTickerSample(factory);


            Console.ReadLine();
        }



        private static async Task RealtimeTickerSample(RealtimeSourceFactory factory)
        {
            var tickerSource = await factory.GetTickerSource(ProductCode.BTCJPY);

            tickerSource.Subscribe(ticker =>
            {
                Console.WriteLine($"{ticker.ProductCode} {ticker.Timestamp.ToLocalTime()} {ticker.BestBid} {ticker.BestAsk} {ticker.LastTradedPrice}");
            });
        }
    }
}
