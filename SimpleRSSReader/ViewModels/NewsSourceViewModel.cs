using RssReader.Common;
using SimpleRSSReader.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleRSSReader.ViewModels
{
    class NewsSourceViewModel : BindableBase
    {
        public NewsSourceViewModel()
        {
            DeleteCommand = new MyCommand<string>(OnDelete);
            AddCommand = new MyCommand<string>(OnAdd);
            AllFeeds.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(HasNoFeeds));
            };

        }
        public ObservableCollection<Feed> AllFeeds { get; set; } = new ObservableCollection<Feed>();
        public Feed NewFeed { get; set; } = new Feed();
        public MyCommand<string> DeleteCommand { get; private set; }
        public MyCommand<string> AddCommand { get; private set; }
        public bool HasNoFeeds => AllFeeds.Count == 0;
        private void OnDelete(string feedLinkToDelete)
        {
            string messageBoxText = $"Are you sure you want to delete feed from uri: {feedLinkToDelete}?";
            MessageBoxResult result = MessageBox.Show(messageBoxText, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(result == MessageBoxResult.Yes)
            {
                Feed feedToDelete = new Feed();
                foreach (var feed in AllFeeds)
                {
                    if (feed.LinkAsString == feedLinkToDelete)
                    {
                        feedToDelete = feed;
                    }
                }
                if (AllFeeds.Remove(feedToDelete))
                {
                    Console.WriteLine($"OnDelete: {feedLinkToDelete} is deleted.");
                    FeedsDataSource.saveAsync(AllFeeds);
                }
                else
                {
                    Console.WriteLine($"OnDelete: {feedLinkToDelete} cant be deleted.");
                }
            }
            
        }
        private void OnAdd(string newFeed)
        {
            if(!IsInError && Link != null && Name != null)
            {
                Feed feed = new Feed();
                feed.LinkAsString = Link;
                feed.Name = Name;
                AllFeeds.Add(feed);
                FeedsDataSource.saveAsync(AllFeeds);
                _name = "";
                _link = "";
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Link));
            }
        }

        public async Task InitializeFeedsAsync()
        {
            (await FeedsDataSource.GetFeedsAsync()).ForEach(feed => AllFeeds.Add(feed));
            foreach (var feed in AllFeeds)
            {
                Console.WriteLine(feed.Name);
            }
        }
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _nameError = true;
                    ErrorMessage = EMPTY_FIELDS;
                    OnPropertyChanged(nameof(ErrorMessage));
                    OnPropertyChanged(nameof(IsInError));
                    OnPropertyChanged(nameof(AddButtonEnabled));
                    return;
                }
                else
                {
                    _nameError = false;
                    OnPropertyChanged(nameof(IsInError));
                    OnPropertyChanged(nameof(AddButtonEnabled));
                    SetProperty(ref _name, value);

                }
            }
        }
        private string _link;
        public String Link
        {
            get => _link;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _linkError = true;
                    ErrorMessage = EMPTY_FIELDS;
                    OnPropertyChanged(nameof(ErrorMessage));
                    OnPropertyChanged(nameof(IsInError));
                    OnPropertyChanged(nameof(AddButtonEnabled));
                    return;
                }

                if (!value.Trim().StartsWith("http://") && !value.Trim().StartsWith("https://"))
                {
                    _linkError = true;
                    ErrorMessage = NOT_HTTP_MESSAGE;
                    OnPropertyChanged(nameof(ErrorMessage));
                    OnPropertyChanged(nameof(IsInError));
                    OnPropertyChanged(nameof(AddButtonEnabled));
                }
                else
                {
                    _linkError = false;
                    OnPropertyChanged(nameof(IsInError));
                    OnPropertyChanged(nameof(AddButtonEnabled));
                    SetProperty(ref _link, value);
                }
            }
        }
        private bool _linkError = true;
        private bool _nameError = true;
        public bool IsInError { get => _linkError || _nameError; }
        public bool AddButtonEnabled => !IsInError;
        public string ErrorMessage { get; set; }

        private readonly string NOT_HTTP_MESSAGE = "You entered invalid link. Link must start with http:// or https://";
        private readonly string EMPTY_FIELDS = "Fields are required.";
    }
}
