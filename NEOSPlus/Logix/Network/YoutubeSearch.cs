using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BaseX;
using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Network;

public enum YoutubeSearchResultType
{
    Condensed, //return a condensed version of the result data, using only a single nested json object for thumbnail quality
    Raw, //return the raw search result data as described here https://developers.google.com/youtube/v3/docs/search#resource
}

[Category("LogiX/Network")]
[NodeName("Search Youtube")]
public class YoutubeSearch : LogixNode
{
    public readonly Input<string> Query;
    public readonly Input<string> APIKeyOverride;
    public readonly Input<int> MaxResults;
    public readonly Input<YoutubeSearchResultType> OutputType;
    
    public readonly Output<JArray> Content;
    public readonly Output<HttpStatusCode> StatusCode;
    private const string YoutubeAPIKey = "AIzaSyAQ_-tgoiKa6X2yiqfY_g7meYwDGs0Vo58";
    private static readonly Uri YoutubeAPIUri = new("https://www.googleapis.com/youtube/v3/search");
    
    [ImpulseTarget]
    public void Request() => StartTask(RunRequest);
    private async Task RunRequest()
    {
        var key = APIKeyOverride.Evaluate();
        if (string.IsNullOrWhiteSpace(key)) key = YoutubeAPIKey;
        var max = MathX.Clamp(MaxResults.Evaluate(), 1, 50);
        var q = Query.Evaluate();
        var outputType = OutputType.Evaluate();
        if (!string.IsNullOrWhiteSpace(q))
        {
            q = ToUrlSlug(q);
            var url = new Uri(YoutubeAPIUri + $"?part=snippet&type=video&key={key}&q={q}&maxResults={max}");
            OnSent.Trigger();
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.UserAgent.Add(Cloud.UserAgent);
                using var response = await Engine.Cloud.SafeHttpClient.SendAsync(request);
                var jsonIn = JObject.Parse(await response.Content.ReadAsStringAsync());
                var items = (JArray) jsonIn["items"];
                switch (outputType)
                {
                    case YoutubeSearchResultType.Raw:
                        Content.Value = items;
                        break;
                    case YoutubeSearchResultType.Condensed:
                        var newJson = new JArray();
                        foreach (var result in items)
                        {
                            var r = new JObject
                            {
                                ["videoId"] = result["id"]["videoId"],
                                ["channelId"] = result["id"]["channelId"],
                                ["channelTitle"] = result["id"]["channelTitle"],
                                ["title"] = result["snippet"]["title"],
                                ["description"] = result["snippet"]["description"],
                                ["published"] = result["snippet"]["publishedAt"],
                                ["thumbnails"] = result["snippet"]["thumbnails"],
                                ["liveStatus"] = result["snippet"]["liveBroadcastContent"]
                            };
                            newJson.Add(r);
                        }
                        Content.Value = newJson;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                StatusCode.Value = response.StatusCode;
                OnResponse.Trigger();
            }
            catch
            {
                Content.Value = null;
                OnError.Trigger();
            }
            Content.Value = null;
            StatusCode.Value = 0;
        }
    }
    public readonly Impulse OnSent;
    public readonly Impulse OnResponse;
    public readonly Impulse OnError;
    
    
    //i didnt make this
    private static string ToUrlSlug(string value)
    {
        value = value.ToLowerInvariant();
        value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);
        value = Regex.Replace(value, @"[^a-z0-9\s-_]", "",RegexOptions.Compiled);
        value = value.Trim('-', '_');
        value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);
        return value ;
    }
}