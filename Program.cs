using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ConsoleApi
{
    class Program
    {

        private static System.Threading.Timer _timer = null;
        public static void Main()
        {
            // Create a Timer object that knows to call our TimerCallback
            // method once every 1000 milliseconds.
            //_timer = new System.Threading.Timer(TimerCallback, null, 0, 1000);

            TimerCall();

            // Wait for the user to hit <Enter>
            Console.ReadLine();
        }

        private static void TimerCall()
        {
            Timer t = new Timer(1000);
            t.AutoReset = true;
            t.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            t.Start();
        }


        private static async void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            // Display the date/time when this method got called.
            await LoadTrades();
        }

        /*
        private static async void TimerCallback(Object o)
        {
            // Display the date/time when this method got called.
            await Task.Run(function: async () => await LoadTrades());
        }*/


        public static async Task LoadTrades()
        {
            //conection
            var client = new HttpClient();
            //web api
            client.BaseAddress = new Uri("https://api-pub.bitfinex.com/v2/");

            //controler api
            var response = await client.GetAsync("trades/tBTCEUR/hist");
            //result
            var result = await response.Content.ReadAsStringAsync();


            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.ReasonPhrase);
                return;
            }

            //if success
            var trade = JsonConvert.DeserializeObject<List<List<double>>>(result);

            List<Trades> tradeobejct = new List<Trades>();

            foreach (var item in trade)
            {
                tradeobejct.Add(new Trades { Id = (int)(item[0]), Mts = (int)(item[1]), Amount = Convert.ToDouble(item[2]), Price = Convert.ToDouble(item[3]) });
            }

            var min = tradeobejct.Min(x => x.Price);
            var max = tradeobejct.Max(x => x.Price);
            var avg = tradeobejct.Average(x => x.Price);

            Console.WriteLine("In TimerCallback: " + DateTime.Now);

            Console.WriteLine("Min:" + min);
            Console.WriteLine("Max:" + max);
            Console.WriteLine("Average:" + avg);
            Console.WriteLine();
        }
    }

}
