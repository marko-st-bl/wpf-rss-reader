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
            if(AllArticles.Count == 0)
            {
                _ = LoadArticlesAsync();
            }
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

        public ObservableCollection<Feed> AllFeeds { get; set; } = new ObservableCollection<Feed>();

        public async Task LoadArticlesAsync()
        {
            IsLoading = true;
            _context.Feed.Articles.Clear();
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
