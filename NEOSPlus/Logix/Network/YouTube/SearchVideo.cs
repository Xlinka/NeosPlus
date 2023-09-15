using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Operators;
using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Network
{
    [Category("LogiX/NeosPlus/Network/YouTube")]
    [NodeName("Search YouTube Videos")]
    public class SearchVideosNode : LogixNode
    {
        public readonly Impulse onSent;
        public readonly Impulse onResponse;
        public readonly Impulse onError;
        public readonly Input<string> apiKey;
        public readonly Input<string> searchQuery;
        public readonly Input<int> maxResults;
        public readonly Output<JArray> items;
        public readonly Output<HttpStatusCode> statusCode;

        [ImpulseTarget]
        public void SendRequest() => StartTask(new Func<Task>(this.RunRequest));

        private async Task RunRequest()
        {
            // Get the values of the inputs
            string apiKeyValue = apiKey.Evaluate();
            string searchQueryValue = searchQuery.Evaluate();
            int maxResultsValue = maxResults.Evaluate();

            // Construct the YouTube Data API search request URL
            string requestUrl = $"https://www.googleapis.com/youtube/v3/search?key={apiKeyValue}&part=snippet&maxResults={maxResultsValue}&type=video&q={searchQueryValue}";

            // Request access to the YouTube API
            if (await Engine.Security.RequestAccessPermission("www.googleapis.com", 443, null) != HostAccessPermission.Allowed)
            {
                UniLog.Warning("Access to the YouTube API was not granted.");
                onError.Trigger();
                return;
            }
            onSent.Trigger();
            // Send the request to YouTube API and wait for the response
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(requestUrl))
            using (HttpContent content = response.Content)
            {
                // Parse the response content as JSON
                string responseString = await content.ReadAsStringAsync();
                JObject jsonResponse = JObject.Parse(responseString);

                // Extract the items from the response JSON
                JArray itemsValue = (JArray)jsonResponse["items"];

                // Return the items as output
                items.Value = itemsValue;
                statusCode.Value = response.StatusCode;
                onResponse.Trigger();
            }
        }
    }
}
