using RssReader.Common;
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
        public FeedViewModel()
        {
            AllArticles = new ObservableCollection<Article>();
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
                    //OnPropertyChanged(nameof(IsLoadingAndNotEmpty));
                }
            }
        }
        
    }
}
