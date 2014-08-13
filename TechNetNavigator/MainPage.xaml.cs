using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

using TechNetNavigator.Model;
using TechNetNavigator.ViewModel;

namespace TechNetNavigator
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void RefreshClick(object sender, EventArgs e)
        {
            App.ViewModel.Refresh();
        }

        private void SearchClick(object sender, EventArgs e)
        {
            SearchTask task = new SearchTask();
            task.SearchQuery = Constants.SearchStr;
            task.Show();
        }

        private void OpenBlogClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/BlogView.xaml", UriKind.Relative));
        }

        private void AboutClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            //MessageBox.Show("Is right");
            base.OnNavigatedTo(e);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListBox)sender).SelectedIndex == -1)
                return;

            var selectedItem = (ItemViewModel)((ListBox)sender).SelectedItem;
            if (selectedItem == null)
                return;

            var detailPage = new DetailPageData
            {
                ItemLink = selectedItem.ItemLink,
                ItemTitle = selectedItem.ItemTitle,
                ItemDetails = selectedItem.ItemDetails,
                ItemImage = selectedItem.ItemImage
            };
            
            // Save the detailpage object which the detailpage will load up
            BaseHelper.SaveSetting(Constants.DetailPageFileName, detailPage);

            NavigationService.Navigate(new Uri("/DetailsView.xaml", UriKind.Relative));

        }

    }
}