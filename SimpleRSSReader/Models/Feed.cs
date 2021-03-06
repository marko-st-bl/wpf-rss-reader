using RssReader.Common;
using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace SimpleRSSReader.Models
{
    [Serializable]
    public class Feed : BindableBase
    {
        public Feed()
        {
            Articles = new ObservableCollection<Article>();
        }
        public Uri Link { get { return _link; } set { if(SetProperty(ref _link, value)) OnPropertyChanged(nameof(LinkAsString)); } }
        private Uri _link;

        [IgnoreDataMember]
        public string LinkAsString
        {
            get { return Link?.OriginalString ?? String.Empty; }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) return;

                if (!value.Trim().StartsWith("http://") && !value.Trim().StartsWith("https://"))
                {
                    return;
                }
                else
                {
                    Uri uri = null;
                    if (Uri.TryCreate(value.Trim(), UriKind.Absolute, out uri)) Link = uri;
                }
            }
        }
        public string Name { get { return _name; } set { SetProperty(ref _name, value); } }
        private string _name;
        public string Description
        {
            get;
            set;
        }
        public DateTime LastSyncDate { get { return _lastSyncDate; } set { SetProperty(ref _lastSyncDate, value); } }
        private DateTime _lastSyncDate;
        public ObservableCollection<Article> Articles { get; }

        public bool IsEmpty => Articles.Count == 0;

        [IgnoreDataMember]
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (SetProperty(ref _isLoading, value))
                {
                    OnPropertyChanged(nameof(IsLoadingAndNotEmpty));
                }
            }
        }
        [IgnoreDataMember] private bool _isLoading;

        public bool IsLoadingAndNotEmpty => IsLoading && !IsEmpty;
    }
}
