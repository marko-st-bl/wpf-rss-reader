using RssReader.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SimpleRSSReader.Models
{
    public class Article : BindableBase
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Author { get; set; }
        public Uri Link { get; set; }
        public DateTimeOffset PublishedDate { get; set; }
        public string PublishedDateFormatted => PublishedDate.ToString("dd MMM yyyy, HH:mm").ToUpper();
    }
}
