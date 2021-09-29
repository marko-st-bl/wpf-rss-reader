using RssReader.Common;
using SimpleRSSReader.Common;
using SimpleRSSReader.Models;
using SimpleRSSReader.ViewModels;
using SimpleRSSReader.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SimpleRSSReader
{
    public class MainWindowViewModel : BindableBase
    {
        public ObservableCollection<Feed> Feeds { get; } = new ObservableCollection<Feed>();

        public MainWindowViewModel()
        {
            _context = new SessionContext();
            MenuItems = _context.MenuItems;

            _menuItemsView = CollectionViewSource.GetDefaultView(MenuItems);
            _menuItemsView.Filter = MenuItemsFilter;
        }

        private readonly ICollectionView _menuItemsView;
        private string _searchKeyword;
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                if (SetProperty(ref _searchKeyword, value))
                {
                    _menuItemsView.Refresh();
                }
            }
        }

        public ObservableCollection<Article> Articles { get; set; }

        public async Task InitializeFeedsAsync()
        {
            //feedViewModel.IsLoading = true;
            (await FeedsDataSource.GetFeedsAsync()).ForEach(feed => Feeds.Add(feed));
            foreach (var feed in Feeds)
            {
                MenuItems.Add(new MenuItem(
                feed.Name,
                "Rss",
                typeof(FeedView),
                new SessionContext() { Feed = feed }
                ));
                _context.Feeds.Add(feed);
            }
            MenuItems.Add(new MenuItem(
                "Sources",
                "SourceBranch",
                typeof(NewsSourceView),
                _context
                ));
        }

        //NEW
        private SessionContext _context;
        private MenuItem _selectedItem;
        private int _selectedIndex;

        public ObservableCollection<MenuItem> MenuItems { get; }
        public MenuItem SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }

        private bool MenuItemsFilter(object obj)
        {
            if (string.IsNullOrWhiteSpace(_searchKeyword))
            {
                return true;
            }

            return obj is MenuItem item
                   && item.Name.ToLower().Contains(_searchKeyword.ToLower());
        }

    }
}
