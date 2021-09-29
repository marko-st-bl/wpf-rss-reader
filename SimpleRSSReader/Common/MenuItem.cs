using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SimpleRSSReader.Common
{
    public class MenuItem : ViewModelBase
    {
        private readonly Type _contentType;
        private readonly object _dataContext;
        private readonly SessionContext _sessionContext;

        private object _content;
        private ScrollBarVisibility _horizontalScrollBarVisibilityRequirement;
        private ScrollBarVisibility _verticalScrollBarVisibilityRequirement = ScrollBarVisibility.Auto;
        private Thickness _marginRequirement = new Thickness(16);

        public MenuItem(string name, string iconName, Type contentType, SessionContext context, object dataContext = null)
        {
            Name = name;
            IconName = iconName;
            _contentType = contentType;
            _dataContext = dataContext;
            _sessionContext = context;
        }

        public string Name { get; }
        public string IconName { get; }


        public object Content => _content = CreateContent();

        public ScrollBarVisibility HorizontalScrollBarVisibilityRequirement
        {
            get => _horizontalScrollBarVisibilityRequirement;
            set => SetProperty(ref _horizontalScrollBarVisibilityRequirement, value);
        }

        public ScrollBarVisibility VerticalScrollBarVisibilityRequirement
        {
            get => _verticalScrollBarVisibilityRequirement;
            set => SetProperty(ref _verticalScrollBarVisibilityRequirement, value);
        }

        public Thickness MarginRequirement
        {
            get => _marginRequirement;
            set => SetProperty(ref _marginRequirement, value);
        }

        private object CreateContent()
        {
            var content = Activator.CreateInstance(_contentType, _sessionContext);
            
            if (_dataContext != null && content is FrameworkElement element)
            {
                element.DataContext = _dataContext;
            }

            return content;
        }
    }
}
