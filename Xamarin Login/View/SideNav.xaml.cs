using Akavache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin_Login.View;

namespace Xamarin_Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SideNav : MasterDetailPage
    {
        public SideNav()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
            Akavache.Registrations.Start("XamarinLogin");
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as SideNavMasterMenuItem;
            if (item == null)
                return;

            if (item.Title == "Logout")
            {
                _ = BlobCache.UserAccount.InvalidateAll();
                _ = BlobCache.UserAccount.Vacuum();
                Application.Current.MainPage = new NavigationPage(new Login());
            }
            else if (item.Title == "Profile")
            {
                Detail = new NavigationPage(new Profile());
                IsPresented = false;

                MasterPage.ListView.SelectedItem = null;
            }
            else if (item.Title == "Dashboard")
            {
                Detail = new NavigationPage(new Dashboard());
                IsPresented = false;

                MasterPage.ListView.SelectedItem = null;
            }
            else if (item.Title == "View All Data")
            {
                Detail = new NavigationPage(new ViewAll());
                IsPresented = false;

                MasterPage.ListView.SelectedItem = null;
            }
            else if (item.Title == "Add Data")
            {
                Detail = new NavigationPage(new AddData());
                IsPresented = false;

                MasterPage.ListView.SelectedItem = null;
            }
            //else
            //{
            //    var page = (Page)Activator.CreateInstance(item.TargetType);
            //    page.Title = item.Title;

            //    Detail = new NavigationPage(page);
            //    IsPresented = false;

            //    MasterPage.ListView.SelectedItem = null;
            //}
        }
    }
}