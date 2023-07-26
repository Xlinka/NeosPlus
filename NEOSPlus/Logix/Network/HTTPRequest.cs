using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BaseX;

namespace FrooxEngine.LogiX.Network
{
    [Category("LogiX/Network")]
    [NodeName("HTTP Request")]
    public class HTTPRequest : LogixNode
    {
        public readonly Impulse onSent;
        public readonly Impulse onResponse;
        public readonly Impulse onError;
        public readonly Input<RequestType> requestType;
        public readonly Input<Uri> requestUri;
        public readonly Input<string> requestHeaders;
        public readonly Input<string> requestBody;
        public readonly Output<string> responseHeaders;
        public readonly Output<string> responseBody;
        public readonly Output<HttpStatusCode> statusCode;

        private static readonly HttpClient client = new HttpClient();

        [ImpulseTarget]
        public void SendRequest() => StartTask(new Func<Task>(this.RunRequest));

        private async Task RunRequest()
        {
            Uri rawRequestUri = requestUri.Evaluate();
            string rawRequestMethodType = requestType.Evaluate().ToString() ?? "GET";
            string rawRequestHeaders = requestHeaders.Evaluate();
            string rawRequestBody = requestBody.Evaluate() ?? "";

            if (rawRequestUri == null || (rawRequestUri.Scheme != "http" && rawRequestUri.Scheme != "https"))
            {
                responseBody.Value = "Error in Uri.";
                onError.Trigger();
                return;
            }

            if (await Engine.Security.RequestAccessPermission(rawRequestUri.Host, rawRequestUri.Port, null) !=
                HostAccessPermission.Allowed)
            {
                responseBody.Value = "Access Request not accepted.";
                onError.Trigger();
                return;
            }

            HttpMethod httpMethod = new HttpMethod(rawRequestMethodType);
            HttpRequestMessage httpRequest = new HttpRequestMessage(httpMethod, rawRequestUri);
            string contentType = null;
            onSent.Trigger();
            try
            {
                if (!string.IsNullOrWhiteSpace(rawRequestHeaders))
                {
                    Dictionary<string, string> headerList = FormatHeaders(rawRequestHeaders);
                    headerList.ToList().ForEach((header) =>
                    {
                        switch (header.Key)
                        {
                            case "Content-Type":
                                contentType = header.Value;
                                break;
                            case "Accept":
                                httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(header.Value));
                                break;
                            case "Connection":
                                httpRequest.Headers.Connection.Add(header.Value);
                                break;
                            case "Content-Length":
                                long lengthValue;
                                long.TryParse(header.Value, out lengthValue);
                                httpRequest.Content.Headers.ContentLength = lengthValue;
                                break;
                            case "Expect":
                                httpRequest.Headers.Expect.Add(new NameValueWithParametersHeaderValue(header.Value));
                                break;
                            case "Date":
                                DateTimeOffset dateValue;
                                DateTimeOffset.TryParse(header.Value, out dateValue);
                                httpRequest.Headers.Date = dateValue;
                                break;
                            case "Host":
                                httpRequest.Headers.Host = header.Value;
                                break;
                            case "If-Modified-Since":
                                DateTimeOffset ifModifiedSinceValue;
                                DateTimeOffset.TryParse(header.Value, out ifModifiedSinceValue);
                                httpRequest.Headers.IfModifiedSince = ifModifiedSinceValue;
                                break;
                            case "Range":
                                string[] rangeValues = header.Value.Split(',');
                                long rangeFrom, rangeTo;
                                long.TryParse(rangeValues[0], out rangeFrom);
                                long.TryParse(rangeValues[1], out rangeTo);
                                httpRequest.Headers.Range = new RangeHeaderValue(rangeFrom, rangeTo);
                                break;
                            case "Referer":
                                httpRequest.Headers.Referrer = new Uri(header.Value);
                                break;
                            case "User-Agent":
                                List<string> userAgentValues = new List<string>(header.Value.Split(' '));
                                userAgentValues.ForEach(value =>
                                {
                                    string[] userAgentParts = value.Split('/');
                                    httpRequest.Headers.UserAgent.Add(
                                        new ProductInfoHeaderValue(userAgentParts[0], userAgentParts[1]));
                                });
                                break;
                            default:
                                httpRequest.Headers.Add(header.Key, header.Value);
                                break;
                        }
                    });
                }

                if (RequestContainsBody(requestType.Evaluate()))
                {
                    httpRequest.Content = new System.Net.Http.StringContent(rawRequestBody, Encoding.UTF8, contentType);
                }

                HttpResponseMessage responseMessage = await client.SendAsync(httpRequest);
                responseBody.Value = await responseMessage.Content.ReadAsStringAsync();
                responseHeaders.Value = StringifyHeaders(responseMessage.Headers);
                statusCode.Value = responseMessage.StatusCode;
                onResponse.Trigger();
            }
            catch (Exception ex)
            {
                responseBody.Value = ex.ToString();
                UniLog.Error(ex.ToString());
                onError.Trigger();
            }

            httpRequest = null;
            httpMethod = null;
        }

        private Dictionary<string, string> FormatHeaders(string headers)
        {
            List<string> rawHeaderList = new List<string>(headers.Trim().Split('\n'));
            Dictionary<string, string> result = new Dictionary<string, string>();
            rawHeaderList.ForEach((header) =>
            {
                string[] splitHeader = header.Split(new[] { ':' }, 2);
                result.Add(splitHeader[0].Trim(), splitHeader[1].Trim());
            });
            return result;
        }

        private string StringifyHeaders(HttpHeaders headers)
        {
            StringBuilder sb = new StringBuilder();
            headers.ToList().ForEach((header) =>
            {
                sb.Append(header.Key);
                sb.Append(": ");
                sb.Append(string.Join(" ", header.Value));
                sb.Append("\n");
            });
            return sb.ToString();
        }

        private bool RequestContainsBody(RequestType requestType)
        {
            switch (requestType)
            {
                case RequestType.GET:
                    return false;
                case RequestType.POST:
                    return true;
                case RequestType.PUT:
                    return true;
                case RequestType.HEAD:
                    return false;
                case RequestType.DELETE:
                    return true;
                case RequestType.PATCH:
                    return true;
                case RequestType.OPTIONS:
                    return false;
                case RequestType.TRACE:
                    return false;
                default:
                    return false;
            }
        }

        public enum RequestType
        {
            GET,
            POST,
            PUT,
            HEAD, //so no head?
            DELETE,
            PATCH,
            OPTIONS,
            TRACE
        }
    }
}