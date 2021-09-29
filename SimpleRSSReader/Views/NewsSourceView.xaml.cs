using SimpleRSSReader.Common;
using SimpleRSSReader.Models;
using SimpleRSSReader.ViewModels;
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

namespace SimpleRSSReader.Views
{
    /// <summary>
    /// Interaction logic for NewsSourceView.xaml
    /// </summary>
    public partial class NewsSourceView : UserControl
    {
        public NewsSourceView(SessionContext context = null)
        {
            DataContext =  new NewsSourceViewModel(context);
            InitializeComponent();
        }
    }
}
