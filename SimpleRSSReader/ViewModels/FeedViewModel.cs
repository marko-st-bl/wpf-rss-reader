using RssReader.Common;
using SimpleRSSReader.Common;
using SimpleRSSReader.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRSSReader.ViewModels
{
    class FeedViewModel : BindableBase
    {
        private SessionContext _context;
        public FeedViewModel(SessionContext context = null)
        {
            _context = context;
            AllArticles = new ObservableCollection<Article>();
            LoadArticlesAsync();
        }
        public ObservableCollection<Article> AllArticles { get; }
        public Article SelectedArticle
        {
            get { return _selectedArticle; }
            set { SetProperty(ref _selectedArticle, value); }
        }
        private Article _selectedArticle;

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (SetProperty(ref _isLoading, value))
                {
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }

        /* public async Task InitializeFeedsAsync()
         {
             IsLoading = true;
             (await FeedsDataSource.GetFeedsAsync()).ForEach(feed => newsSourceViewModel.AllFeeds.Add(feed));
             foreach (var feed in newsSourceViewModel.AllFeeds)
             {
                 //Console.WriteLine(feed.Name);
                 await FeedsDataSource.GetFeedAsync(feed);
                 foreach (var article in feed.Articles)
                 {
                     feedViewModel.AllArticles.Add(article);
                 }
             }
             feedViewModel.SelectedArticle = feedViewModel.AllArticles.Count == 0 ? null : feedViewModel.AllArticles[0];
             feedViewModel.IsLoading = false;
         }*/
        public ObservableCollection<Feed> AllFeeds { get; set; } = new ObservableCollection<Feed>();

        public async Task LoadArticlesAsync()
        {
            IsLoading = true;
            await FeedsDataSource.GetFeedAsync(_context.Feed);
            foreach (var article in _context.Feed.Articles)
            {
                AllArticles.Add(article);
            }
            SelectedArticle = AllArticles.Count == 0 ? null : AllArticles[0];
            IsLoading = false;
        }

    }
}
