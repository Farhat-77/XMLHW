using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace XMLHW
{
    public class Item
    {
        private object gvRss;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.PopulateRssFeed();
        }
        private void PopulateRssFeed()
        {
            string RssFeedUrl = ConfigurationManager.AppSettings["RssFeedUrl"];
            string RssMaxNews = ConfigurationManager.AppSettings["RssMaxNews"];
            List<Feeds> feeds = new List<Feeds>();
            try
            {
                XDocument xDoc = new XDocument();
                xDoc = XDocument.Load(RssFeedUrl);
                string pubDate = "01/3/2021 09:43:17 PM";
                DateTime dt = DateTime.ParseExact(pubDate, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
                var items = (from x in xDoc.Descendants("item")
                             select new
                             {
                                 title = x.Element("title").Value,
                                 link = x.Element("link").Value,
                                 pubDate = dt.ToString("M/d/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                                 description = x.Element("description").Value,
                             }).Take(10).ToList();
                if (items != null)
                {
                    foreach (var i in items)
                    {
                        Feeds f = new Feeds
                        {
                            Title = i.title,
                            Link = i.link,
                            PublishDate = i.pubDate,
                            Description = i.description
                        };
                        feeds.Add(f);
                    }
                }
                gvRss.DataSource = feeds;
                gvRss.DataBind();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
