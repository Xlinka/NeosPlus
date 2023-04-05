using FrooxEngine;
using FrooxEngine.LogiX;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NEOSPlus.Logix.Web3.NFT
{
    [NodeName("NFT Data (OpenSea V2)")]
    [Category("LogiX/Web3/NFT")]
    public class NFTDataOpenSeaV2 : LogixNode
    {
        private const string OpenSeaAPIEndpoint = "https://api.opensea.io/api/v2/asset/{0}/{1}/";

        public readonly Input<string> ContractAddress;
        public readonly Input<string> TokenId;
        public readonly Input<string> APIKey;
        public readonly Output<string> Name;
        public readonly Output<string> Description;
        public readonly Output<string> ImageUrl;
        public readonly Output<string> AnimationUrl;
        public readonly Output<string> ExternalLink;
        public readonly Output<string> OwnerAddress;
        public readonly Output<string> CreatorAddress;
        public readonly Output<HttpStatusCode> StatusCode;

        public readonly Impulse OnError;
        public readonly Impulse OnSuccess;
        public readonly Impulse OnFail;
        public readonly Impulse OnResponse;

        [ImpulseTarget]
        public void Request() => StartTask(RunRequest);

        private async Task RunRequest()
        {
            var url = new Uri(string.Format(OpenSeaAPIEndpoint, ContractAddress.EvaluateRaw(), TokenId.EvaluateRaw()));

            if (url.Scheme != Uri.UriSchemeHttp && url.Scheme != Uri.UriSchemeHttps)
            {
                OnError.Trigger();
                return;
            }

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            var apiKey = APIKey.EvaluateRaw();

            if (!string.IsNullOrEmpty(apiKey))
            {
                request.Headers.Add("X-API-KEY", apiKey);
            }

            try
            {
                using var response = await Engine.Cloud.SafeHttpClient.SendAsync(request);
                StatusCode.Value = response.StatusCode;
                OnResponse.Trigger();

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    dynamic responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);

                    Name.Value = responseJson.name;
                    Description.Value = responseJson.description;
                    ImageUrl.Value = responseJson.image_url;
                    AnimationUrl.Value = responseJson.animation_url;
                    ExternalLink.Value = responseJson.external_link;
                    OwnerAddress.Value = responseJson.owner.address;
                    CreatorAddress.Value = responseJson.creator.address;

                    OnSuccess.Trigger();
                }
                else
                {
                    OnFail.Trigger();
                }
            }
            catch (HttpRequestException)
            {
                OnError.Trigger();
            }
        }
    }
}
