using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

using TechNetNavigator.Model;
namespace TechNetNavigator
{
    public partial class DetailsView : PhoneApplicationPage
    {
        private DetailPageData _detailItem;
        public DetailsView()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(DetailsView_Loaded);
        }
        private void HomeClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
        private void SearchClick(object sender, EventArgs e)
        {
            SearchTask task = new SearchTask();
            task.SearchQuery = Constants.SearchStr + " " +_detailItem.ItemTitle;
            task.Show();
        }
        private void MessageClick(object sender, EventArgs e)
        {
            SmsComposeTask task = new SmsComposeTask();
            //task.To = "";
            string smsText = "@" + Constants.SearchStr;
            smsText += "\n" + _detailItem.ItemTitle + ": " + _detailItem.ItemDetails;
            smsText += "\n链接：" + _detailItem.ItemLink;

            task.Body = smsText;
            task.Show();
        }
        private void EmailClick(object sender, EventArgs e)
        {
            EmailComposeTask task = new EmailComposeTask();
            //task.To = "";
            //task.Cc = "";
            string emailTitle = Constants.SearchStr;
            emailTitle += _detailItem.ItemTitle;

            string emailText = "@" + Constants.SearchStr;
            emailText += "\n\n" + _detailItem.ItemTitle + "\n\n" + _detailItem.ItemDetails;
            emailText += "\n\n链接：" + _detailItem.ItemLink;

            task.Subject = emailTitle;
            task.Body = emailText; 
            task.Show();
        }

        void DetailsView_Loaded(object sender, RoutedEventArgs e)
        {
            Uri url = new Uri(_detailItem.ItemLink);
            //html.Append(MakeLinks(_detailItem.ItemLink));
            //webBrowser.NavigateToString(html.ToString());
            webBrowser.Navigate(url);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _detailItem = BaseHelper.LoadSetting<DetailPageData>(Constants.DetailPageFileName);
            if (_detailItem == null)
            {
                //MessageBox.Show("URL为空");
                return;
            }
        }

        private static string MakeLinks(string txt)
        {
            var regx = new Regex(@"http(s)?://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&amp;\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?", RegexOptions.IgnoreCase);
            var mactches = regx.Matches(txt);

            return mactches.Cast<Match>().Aggregate(txt, (current, match) => current.Replace(match.Value, "<a href='" + match.Value + "'>" + match.Value + "</a>"));
        }

        private void WebBrowserNavigating(object sender, NavigatingEventArgs e)
        {
            ProgressBar.Visibility = Visibility.Visible;
            ProgressBar.IsIndeterminate = true;
        }

        private void WebBrowserNavigated(object sender, NavigationEventArgs e)
        {
            ProgressBar.Visibility = Visibility.Collapsed;
            ProgressBar.IsIndeterminate = false;
        }

    }
}