using FrooxEngine.LogiX;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FrooxEngine.LogiX.Network
{
    public enum Currency
    {
        USD,
        EUR,
        GBP,
        JPY,
        AUD,
        CAD
    }

    [NodeName("Coingecko")]
    [Category("LogiX/Network/Crypto")]
    public class Coingecko : LogixNode
    {
        private const string priceEndpoint = "https://api.coingecko.com/api/v3/simple/price?ids={0}&vs_currencies={1}";
        public readonly Input<string> CryptocurrencyName;
        public readonly Input<Currency> Currency;
        public readonly Output<string> Price;
        public readonly Impulse OnSent;
        public readonly Impulse OnResponse;
        public readonly Impulse OnError;

        [ImpulseTarget]
        public void Request()
        {
            StartTask(RunRequest);
        }

        private async Task RunRequest()
        {
            Uri url = new Uri(string.Format(priceEndpoint, CryptocurrencyName.EvaluateRaw(), Currency.EvaluateRaw().ToString().ToLower()));
            if (url == null || (url.Scheme != "http" && url.Scheme != "https"))
            {
                return;
            }

            OnSent.Trigger();

            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                try
                {
                    HttpResponseMessage response = await Engine.Cloud.SafeHttpClient.SendAsync(request);
                    try
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        dynamic responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
                        string price = responseJson[CryptocurrencyName.EvaluateRaw()][Currency.EvaluateRaw().ToString().ToLower()];
                        Price.Value = price;
                    }
                    finally
                    {
                        ((IDisposable)response)?.Dispose();
                    }
                }
                finally
                {
                    ((IDisposable)request)?.Dispose();
                }
            }
            catch (HttpRequestException ex)
            {
                OnError.Trigger();
            }

            OnResponse.Trigger();
        }
    }
}