using FrooxEngine.LogiX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CloudX.Shared;
using FrooxEngine;
using System.Net;

namespace FrooxEngine.LogiX.Network
{
	[NodeName("OpenSea")]
	[Category(new string[] { "LogiX/Network" })]

	public class OpenSea : LogixNode
    {
        private const string assetEndpoint = "https://api.opensea.io/api/v1/asset/{0}/{1}/?include_orders=false";
        public readonly Input<string> AssetContractAddress;
        public readonly Input<string> TokenID;
		public readonly Output<string> Content;
		public readonly Output<HttpStatusCode> StatusCode;
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
			string googleUserInfo = string.Empty;
			Uri url = new Uri(string.Format(assetEndpoint, AssetContractAddress.EvaluateRaw(), TokenID.EvaluateRaw()));
			if (url == null || (url.Scheme != "http" && url.Scheme != "https") || await base.Engine.Security.RequestAccessPermission(url.Host, url.Port, null) == HostAccessPermission.Denied)
			{
				return;
			}

			OnSent.Trigger();

			// Get OpenSee info
			try
			{
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
				try
				{
					request.Headers.UserAgent.Add(Cloud.UserAgent);
					HttpResponseMessage response = await Engine.Cloud.SafeHttpClient.SendAsync(request);
					try
					{
						googleUserInfo = await response.Content.ReadAsStringAsync();
						StatusCode.Value = response.StatusCode;
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
			catch (HttpRequestException val)
			{
				HttpRequestException val2 = val;
				Content.Value = val2.ToString();
				OnError.Trigger();
			}

			if (googleUserInfo is null)
            {
				return;
            }

			// Get GoogleUserContent
			try
			{
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, googleUserInfo);
				try
				{
					request.Headers.UserAgent.Add(Cloud.UserAgent);
					HttpResponseMessage response = await Engine.Cloud.SafeHttpClient.SendAsync(request);
					try
					{
						Output<string> content = Content;
						content.Value = await response.Content.ReadAsStringAsync();
						StatusCode.Value = response.StatusCode;
						OnResponse.Trigger();
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
			catch (HttpRequestException val)
			{
				HttpRequestException val2 = val;
				Content.Value = val2.ToString();
				OnError.Trigger();
			}
			Content.Value = null;
			StatusCode.Value = 0;
		}
		protected override void NotifyOutputsOfChange()
		{
			((IOutputElement)Content).NotifyChange();
			((IOutputElement)StatusCode).NotifyChange();
		}
	}
}
