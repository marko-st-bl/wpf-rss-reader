using SimpleRSSReader.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRSSReader.Common
{
    public class SessionContext : ViewModelBase
    {
        public SessionContext()
        {
        }
        public Feed Feed { get; set; }
        public List<Feed> Feeds { get; set; } = new List<Feed>();
        public ObservableCollection<MenuItem> MenuItems { get; } = new ObservableCollection<MenuItem>();
    }
}
