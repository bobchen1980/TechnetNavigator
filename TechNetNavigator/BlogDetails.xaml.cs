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
using System.Windows.Navigation;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;

using TechNetNavigator.Model;
using TechNetNavigator.BlogModel;

namespace TechNetNavigator
{
    public partial class BlogDetails : PhoneApplicationPage
    {
        private ObservableCollection<BlogPageData> BlogDetailItem { get; set; }
        private BlogPageData blogDetail;

        public BlogDetails()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(DetailsView_Loaded);
        }

        void DetailsView_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(blogDetail.ItemText);
            BlogDetailItem = new ObservableCollection<BlogPageData>();

            BlogDetailItem.Add(blogDetail);

            BlogContent.ItemsSource = BlogDetailItem;
            PageTitle.Text = "@" + blogDetail.ItemUser; 
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            blogDetail = BaseHelper.LoadSetting<BlogPageData>(Constants.BlogDetailFileName);
            if (blogDetail == null)
            {
                //MessageBox.Show("URL为空");
                return;
            }
        }

        private void HomeClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
        private void SearchClick(object sender, EventArgs e)
        {
            SearchTask task = new SearchTask();
            task.SearchQuery = blogDetail.ItemUser;
            task.Show();
        }
        private void MessageClick(object sender, EventArgs e)
        {
            SmsComposeTask task = new SmsComposeTask();
            //task.To = "";
            string smsText = "@" + Constants.SearchStr;
            smsText += "\n" + blogDetail.ItemText ;
            smsText += "\n 链接：" + blogDetail.ItemLink;

            task.Body = smsText;
            task.Show();
        }
        private void EmailClick(object sender, EventArgs e)
        {
            EmailComposeTask task = new EmailComposeTask();
            //task.To = "";
            //task.Cc = "";
            string emailTitle = Constants.SearchStr;
            emailTitle += blogDetail.ItemUser;

            string emailText = "@" + Constants.SearchStr;
            if (String.IsNullOrEmpty(blogDetail.Itemthumbnail))
                emailText += "\n\n" + blogDetail.ItemText;
            else
                emailText += "\n\n" + blogDetail.ItemText + "\n\n<img src=" + blogDetail.Itemthumbnail + ">";
            emailText += "\n\n链接：" + blogDetail.ItemLink;

            task.Subject = emailTitle;
            task.Body = emailText;
            task.Show();
        }

    }
}