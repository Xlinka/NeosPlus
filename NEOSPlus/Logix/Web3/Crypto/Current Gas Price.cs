using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace FrooxEngine.LogiX.Web3
{
    /// <summary>
    /// was going to use the netherium library but i didnt want to add more external dependencies to this plugin. - xlinka
    /// </summary>
    [Category("LogiX/NeosPlus/Web3/Crypto")]
    [NodeName("Current Gas Price")]
    public class GasPriceNode : LogixNode
    {
        public readonly Input<string> RpcUrl;
        public readonly Output<decimal> GasPrice;
        public readonly Impulse OnFetch;
        public readonly Impulse OnDone;
        public readonly Impulse OnFail;

        [ImpulseTarget]
        public async void FetchGasPrice()

        {
            OnFetch.Trigger();

            try
            {
                string rpcUrl = RpcUrl.Evaluate();
                using HttpClient httpClient = new HttpClient();
                var requestContent = new StringContent("{\"jsonrpc\":\"2.0\",\"method\":\"eth_gasPrice\",\"params\":[],\"id\":1}", Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(rpcUrl, requestContent);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                JsonDocument jsonDocument = JsonDocument.Parse(responseBody);
                string gasPriceHex = jsonDocument.RootElement.GetProperty("result").GetString();
                decimal gasPriceWei = Convert.ToDecimal(Convert.ToInt64(gasPriceHex, 16));
                decimal gasPriceGwei = gasPriceWei / 1_000_000_000m; // Convert Wei to Gwei (1 Gwei = 1e9 Wei)

                GasPrice.Value = gasPriceGwei;
            }
            catch (Exception)
            {
                OnFail.Trigger();
                return;
            }
            OnDone.Trigger();
        }
    }
}