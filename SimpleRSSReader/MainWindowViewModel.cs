using RssReader.Common;
using SimpleRSSReader.Models;
using SimpleRSSReader.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SimpleRSSReader
{
    public class MainWindowViewModel : BindableBase
    {
        public ObservableCollection<Feed> Feeds { get; }
        public MainWindowViewModel()
        {
            NavCommand = new MyCommand<string>(OnNav);
            CurrentViewModel = feedViewModel;

        }


        private FeedViewModel feedViewModel = new FeedViewModel();
        private NewsSourceViewModel newsSourceViewModel = new NewsSourceViewModel();
        public ObservableCollection<Article> Articles { get; set; }

        private BindableBase _CurrentViewModel;
        public BindableBase CurrentViewModel { get { return _CurrentViewModel; } set { SetProperty(ref _CurrentViewModel, value); } }
        public MyCommand<string> NavCommand { get; private set; }
        private void OnNav(string dest)
        {
            Console.WriteLine($"OnNav: {dest}");
            switch (dest)
            {
                case "feed":
                    CurrentViewModel = feedViewModel;
                    MainWindow.Current.MainView.Content = feedViewModel;
                    break;
                case "news_sources":
                    CurrentViewModel = newsSourceViewModel;
                    MainWindow.Current.MainView.Content = newsSourceViewModel;
                    break;
                default:
                    CurrentViewModel = feedViewModel;
                    break;
            }
        }


        public async Task InitializeFeedsAsync()
        {
            feedViewModel.IsLoading = true;
            (await FeedsDataSource.GetFeedsAsync()).ForEach(feed => newsSourceViewModel.AllFeeds.Add(feed));
            foreach (var feed in newsSourceViewModel.AllFeeds)
            {
                //Console.WriteLine(feed.Name);
                await FeedsDataSource.GetFeedAsync(feed);
                foreach(var article in feed.Articles)
                {
                    feedViewModel.AllArticles.Add(article);
                }
            }
            feedViewModel.SelectedArticle = feedViewModel.AllArticles.Count == 0 ? null : feedViewModel.AllArticles[0];
            feedViewModel.IsLoading = false;
        }

    }
}
