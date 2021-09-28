using RssReader.Common;
using SimpleRSSReader.Models;
using SimpleRSSReader.ViewModels;
using SimpleRSSReader.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleRSSReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel MainViewModel { get; } = new MainWindowViewModel();
        public static MainWindow Current = null;
        public MainWindow()
        {
            InitializeComponent();
            Current = this;
            DataContext = MainViewModel;
            Current.MainView.Content = MainViewModel.CurrentViewModel;
            this.Loaded += async (s, e) =>
            {
                await MainViewModel.InitializeFeedsAsync();
            };
            
        }

    }
}
