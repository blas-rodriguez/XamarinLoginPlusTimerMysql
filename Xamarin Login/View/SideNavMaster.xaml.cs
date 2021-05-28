using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin_Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SideNavMaster : ContentPage
    {
        public ListView ListView;

        public SideNavMaster()
        {
            InitializeComponent();

            BindingContext = new SideNavMasterViewModel();
            ListView = MenuItemsListView;

            AppBarTitle.FontSize = 32;
            AppBarTitle.FontAttributes = FontAttributes.Bold;
            AppBarTitle.TextColor = Xamarin.Forms.Color.White;
        }

        private class SideNavMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<SideNavMasterMenuItem> MenuItems { get; set; }

            public SideNavMasterViewModel()
            {
                MenuItems = new ObservableCollection<SideNavMasterMenuItem>(new[]
                {
                   // new SideNavMasterMenuItem { Id = 0, Title = "Profile" },
                   // new SideNavMasterMenuItem { Id = 1, Title = "Dashboard" },
                   // new SideNavMasterMenuItem { Id = 2, Title = "View All Data" },
                   // new SideNavMasterMenuItem { Id = 3, Title = "Add Data" },
                    new SideNavMasterMenuItem { Id = 4, Title = "Logout" },
                });
            }

            #region INotifyPropertyChanged Implementation

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            #endregion INotifyPropertyChanged Implementation
        }
    }
}