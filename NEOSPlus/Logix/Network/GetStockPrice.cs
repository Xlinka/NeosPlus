using System;
using System.Net.Http;
using System.Threading.Tasks;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Network
{
    [Category("LogiX/NeosPlus/Network")]
    [NodeName("Get Stock Price")]
    public class GetStockPriceNode : LogixNode
    {
        public readonly Impulse onSent;
        public readonly Impulse onResponse;
        public readonly Impulse onError;
        public readonly Input<string> stockSymbol;
        public readonly Output<decimal> stockPrice;
        public readonly Output<int> statusCode;

        [ImpulseTarget]
        public void SendRequest() => StartTask(new Func<Task>(this.RunRequest));

        private async Task RunRequest()
        {
            string symbol = stockSymbol.Evaluate();
            if (string.IsNullOrEmpty(symbol))
            {
                UniLog.Warning("Stock symbol is empty.");
                onError.Trigger();
                return;
            }

            string requestUrl = $"https://query1.finance.yahoo.com/v8/finance/chart/{symbol}?interval=1d";

            if (await Engine.Security.RequestAccessPermission("query1.finance.yahoo.com", 443, null) != HostAccessPermission.Allowed)
            {
                UniLog.Warning("Access to the Yahoo Finance API was not granted.");
                onError.Trigger();
                return;
            }

            // Send the request to Yahoo Finance API and wait for the response
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(requestUrl))
            using (HttpContent content = response.Content)
            {
                string responseString = await content.ReadAsStringAsync();
                JObject jsonResponse = JObject.Parse(responseString);

                // Extract the stock price from the response JSON
                JArray resultArray = (JArray)jsonResponse["chart"]["result"];
                if (resultArray.Count == 0)
                {
                    UniLog.Warning($"Failed to get stock price for symbol '{symbol}'.");
                    onError.Trigger();
                    return;
                }

                JObject result = (JObject)resultArray[0];
                JObject meta = (JObject)result["meta"];
                decimal price = (decimal)meta["regularMarketPrice"];

                // Return the stock price as output
                stockPrice.Value = price;
                statusCode.Value = (int)response.StatusCode;
                onResponse.Trigger();
            }
        }
    }
}
