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

		[ImpulseTarget]
		public void SendRequest() => StartTask(new Func<Task>(this.RunRequest));

		private async Task RunRequest()
		{
			// A bunch of value parsing and making sure things don't error out if a user doesn't put an input in to the node.
			Uri rawRequestUri = requestUri.Evaluate();
			string rawRequestMethodType = requestType.Evaluate().ToString() ?? "GET";
			string rawRequestHeaders = requestHeaders.Evaluate();
			string rawRequestBody = requestBody.Evaluate() ?? "";

			// Making sure that the Uri has to have 'http://' or 'https://' at the start.
			if (rawRequestUri == null || (rawRequestUri.Scheme != "http" && rawRequestUri.Scheme != "https"))
			{
				responseBody.Value = "Error in Uri.";
				onError.Trigger();
				return;
			}

			//Requesting Access to remote uri
			if (await Engine.Security.RequestAccessPermission(rawRequestUri.Host, rawRequestUri.Port, null) != HostAccessPermission.Allowed)
			{
				responseBody.Value = "Access Request not accepted.";
				onError.Trigger();
				return;
			}

			//Setting up HTTP request (the dumb OOP way...)
			HttpMethod httpMethod = new HttpMethod(rawRequestMethodType);
			HttpRequestMessage httpRequest = new HttpRequestMessage(httpMethod, rawRequestUri);
			string contentType = null;
			//Didn't exactly know where to put this trigger. This seems like the best spot.
			onSent.Trigger();
			//This was supposed to be easy... Just split up the headers string, and add them to the headers. But noooo... Microsoft in their infinite wisdom
			//wanted to split up headers in to "restricted" headers which can only be added via their respective properties,
			//and also wanted to differentiate between "request headers" and "content headers".
			try
			{
				if (!string.IsNullOrWhiteSpace(rawRequestHeaders))
				{
					Dictionary<string, string> headerList = FormatHeaders(rawRequestHeaders);
					headerList.ToList().ForEach((header) =>
					{
						switch (header.Key)
						{
							//fuck Content-Type specifically, as the property didn't even work... I needed to put it in the request AFTERWARDS (ln 124)
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
								userAgentValues.ForEach(value => { 
									string[] userAgentParts = value.Split('/');
									httpRequest.Headers.UserAgent.Add(new ProductInfoHeaderValue(userAgentParts[0], userAgentParts[1]));
								});
								break;
							default:
								//It should've been this easy for all of them...
								httpRequest.Headers.Add(header.Key, header.Value);
								break;
						}
					});
				}
				if (RequestContainsBody(requestType.Evaluate()))
				{
					httpRequest.Content = new System.Net.Http.StringContent(rawRequestBody, Encoding.UTF8, contentType);
				}
				HttpResponseMessage responseMessage = await Engine.Cloud.SafeHttpClient.SendAsync(httpRequest);
				responseBody.Value = responseMessage.Content.ReadAsStringAsync().Result;
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
			//Is this necessary?
			httpRequest = null;
			httpMethod = null;
		}

		//Helper function to format the "one big long string of headers" to "a list of key-value pairs of headers".
		//Will likely change this to collecions once support is added in NeosPlus.
		//Example valid input string for headers:
		//  "Authroization: bingus
		//  Content-Type:application/json
		//  TOTP : 696969"
		private Dictionary<string, string> FormatHeaders(string headers)
		{
			List<string> rawHeaderList = new List<string>(headers.Split('\n'));
			Dictionary<string, string> result = new Dictionary<string, string>();
			rawHeaderList.ForEach((header) =>
			{
				string[] splitHeader = header.Split(new[] {':'}, 2);

				result.Add(splitHeader[0].Trim(), splitHeader[1].Trim());
			});
			return result;
		}

		//Helper function to allow Neos to output a new-line separated string of response headers. Will look very similar to example above.
		private string StringifyHeaders(HttpHeaders responseHeaders)
		{
			string result = "";
			responseHeaders.ToList().ForEach((header) =>
			{
				result += header.Key + ": " + string.Join(",", header.Value.ToArray());
				if (!responseHeaders.Last().Equals(header))
				{
					result += "\n";
				}
			});
			return result;
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
