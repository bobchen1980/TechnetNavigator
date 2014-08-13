using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;

using TechNetNavigator.Model;
using TechNetNavigator.BlogModel;

namespace TechNetNavigator
{
    public partial class BlogView : PhoneApplicationPage
    {
        public BlogView()
        {
            InitializeComponent();
            DataContext = App.BlogModel;
            this.Loaded += new RoutedEventHandler(BlogPage_Loaded);
        }

        private void BlogPage_Loaded(object sender, RoutedEventArgs e)
        {
            App.BlogModel.LoadData();
        }
        
        private void HomeClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
        private void RefreshClick(object sender, EventArgs e)
        {
            App.BlogModel.Refresh();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListBox)sender).SelectedIndex == -1)
                return;

            var selectedItem = (BlogItemModel)((ListBox)sender).SelectedItem;
            if (selectedItem == null)
                return;

            var blogDetail = new BlogPageData
            {
                ItemLink = selectedItem.ItemLink,
                ItemUser = selectedItem.ItemUser,
                ItemText  = selectedItem.ItemText,
                ItemImage  = selectedItem.ItemImage,
                ItemDate  = selectedItem.ItemDate,
                Itemthumbnail = selectedItem.ItemOriginalpic,
                ReItemMergedText  = selectedItem.ReItemMergedText,
                ReItemDate  = selectedItem.ReItemDate,
                ReItemImage = selectedItem.ReItemOriginalpic

            };

            // Save the detailpage object which the detailpage will load up
            BaseHelper.SaveSetting(Constants.BlogDetailFileName, blogDetail);

            NavigationService.Navigate(new Uri("/BlogDetails.xaml", UriKind.Relative));

        }

    }
}