using SimpleRSSReader.Models;
using System;
using System.Collections.Generic;
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
using Microsoft.Web.WebView2.Core;

namespace SimpleRSSReader.Views
{
    /// <summary>
    /// Interaction logic for FeedView.xaml
    /// </summary>
    public partial class FeedView : UserControl
    {
        public FeedView()
        {
            InitializeComponent();
        }

        private void LstOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (webView != null && webView.CoreWebView2 != null && articlesLst.SelectedItem != null)
            {
                webView.Source = new Uri(((Article)articlesLst.SelectedItem).Link.ToString());
                webView.CoreWebView2.Navigate(((Article)articlesLst.SelectedItem).Link.ToString());
                Console.WriteLine(App.Current.MainWindow.DataContext);
            }
        }
    }
}
