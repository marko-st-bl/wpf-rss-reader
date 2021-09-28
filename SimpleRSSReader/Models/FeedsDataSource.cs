using Microsoft.Toolkit.Parsers.Rss;
using SimpleRSSReader.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace SimpleRSSReader.Models
{
    public static class FeedsDataSource
    {

        public static async Task<bool> GetFeedAsync(Feed feedModel)
        {
            string feed = null;
            using(var client = new HttpClient())
            {
                try
                {
                    feed = await client.GetStringAsync(feedModel.Link);
                    feedModel.LastSyncDate = DateTime.Now;
                }
                catch
                {
                    return false;
                }

                if(feed!=null)
                {
                    var parser = new RssParser();
                    var rss = parser.Parse(feed);
                    
                    foreach(var element in rss)
                    {
                        Article newArticle = new Article();
                        newArticle.Title = element.Title;
                        newArticle.Summary = element.Summary;
                        newArticle.PublishedDate = element.PublishDate;
                        newArticle.Link = new Uri(element.FeedUrl);
                        feedModel.Articles.Add(newArticle);
                        feedModel.LastSyncDate = DateTime.Now;
                        //Console.WriteLine(element.FeedUrl);
                    }
                }
            }
            return true;
        }

        public static async Task saveAsync(ObservableCollection<Feed> feeds)
        {
            var file = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\feeds.dat";
            Console.WriteLine($"SaveAsync: {file}");
            byte[] array = Serializer.Serialize(feeds.Select(feed => new[] { feed.Name, feed.Link.ToString() }).ToArray());
            File.WriteAllBytes(file, array);
        }

        public static async Task<List<Feed>> GetFeedsAsync()
        {
            List<Feed> feeds = new List<Feed>();
            var file = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\feeds.dat";
            if (File.Exists(file))
            {
                var bytes = File.ReadAllBytes(file);
                var feedData = Serializer.Deserialize<string[][]>(bytes);
                foreach(var feed in feedData)
                {
                    var feedM = new Feed { Name = feed[0], Link = new Uri(feed[1]) };
                    feeds.Add(feedM);
                }
            }
            return feeds;
        }
    }
}
