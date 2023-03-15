using BaseX;
using FrooxEngine.LogiX;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FrooxEngine.LogiX.Web3
{
    public enum Platform
    {
        UniswapV2,
        UniswapV3
    }

    [NodeName("Market Token Price")]
    [Category("LogiX/Web3/Crypto")]
    public class TokenPrice : LogixNode
    {
        private const string graphApiUrlV2 = "https://api.thegraph.com/subgraphs/name/uniswap/uniswap-v2";
        private const string graphApiUrlV3 = "https://api.thegraph.com/subgraphs/name/uniswap/uniswap-v3";
        public readonly Input<string> TokenID;
        public readonly Input<string> PoolID;
        public readonly Input<Platform> Market;
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
            OnSent.Trigger();

            try
            {
                string tokenID = TokenID.EvaluateRaw().Replace("\"", "\\\"");
                string poolID = PoolID.EvaluateRaw().Replace("\"", "\\\"");
                string query;
                string graphApiUrl;

                switch (Market.Evaluate())
                {
                    case Platform.UniswapV2:
                        graphApiUrl = graphApiUrlV2;
                        query = $"{{\"query\":\"{{token(id: \\\"{tokenID}\\\") {{id symbol name derivedETH}} bundle(id: \\\"1\\\") {{id ethPrice}} }}\"}}";
                        break;
                    case Platform.UniswapV3:
                        if (string.IsNullOrEmpty(poolID))
                        {
                            UniLog.Log("Pool ID is required for Uniswap V3.");
                            OnError.Trigger();
                            return;
                        }
                        graphApiUrl = graphApiUrlV3;
                        query = $"{{\"query\":\"{{pool(id: \\\"{poolID}\\\") {{id token0 {{id symbol}} token1 {{id symbol}} token0Price token1Price}} }}\"}}";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Platform), "Unsupported platform selected.");
                }

                using var requestContent = new System.Net.Http.StringContent(query, Encoding.UTF8, "application/json");
                using var request = new HttpRequestMessage(HttpMethod.Post, graphApiUrl) { Content = requestContent };

                HttpResponseMessage response = await Engine.Cloud.SafeHttpClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                dynamic responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);

                if (Market.Evaluate() == Platform.UniswapV2)
                {
                    string derivedETH = responseJson.data.token.derivedETH;
                    string ethPrice = responseJson.data.bundle.ethPrice;
                    Price.Value = (decimal.Parse(derivedETH) * decimal.Parse(ethPrice)).ToString("0.####");
                }
                else // UniswapV3
                {
                    string token0Price = responseJson.data.pool.token0Price;
                    Price.Value = decimal.Parse(token0Price).ToString("0.####");
                }
            }
            catch (HttpRequestException ex)
            {
                OnError.Trigger();
            }
            catch (Exception ex)
            {
                UniLog.Log($"Exception running asynchronous task: {ex}");
                OnError.Trigger();
            }

            OnResponse.Trigger();
        }
    }
}