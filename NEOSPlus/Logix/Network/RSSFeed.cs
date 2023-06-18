using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;
using FrooxEngine;
using FrooxEngine.LogiX;
using BaseX;
using System.Xml;

namespace LogiX.Network
{
    [Category("LogiX/Network")]
    [NodeName("RSS Feed")]
    public class RSSFeedNode : LogixNode
    {
        public readonly Input<Uri> FeedURL;
        public readonly Output<JArray> FeedItems;

        protected override void OnEvaluate()
        {
            Uri feedURL = FeedURL.Evaluate();
            JArray items = new JArray();

            try
            {
                // Retrieve RSS feed content
                WebClient client = new WebClient();
                string feedContent = client.DownloadString(feedURL);

                // Parse the XML content
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(feedContent);

                // Extract feed items
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//item");
                foreach (XmlNode itemNode in itemNodes)
                {
                    JObject item = new JObject();
                    item["title"] = itemNode.SelectSingleNode("title")?.InnerText;
                    item["description"] = itemNode.SelectSingleNode("description")?.InnerText;
                    item["pubDate"] = itemNode.SelectSingleNode("pubDate")?.InnerText;
                    item["link"] = itemNode.SelectSingleNode("link")?.InnerText;
                    items.Add(item);
                }
            }
            catch (Exception ex)
            {
                UniLog.Log("Error retrieving or parsing RSS feed: " + ex.Message);
            }

            FeedItems.Value = items;
        }
    }
}
