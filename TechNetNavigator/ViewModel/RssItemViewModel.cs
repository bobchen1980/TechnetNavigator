using System.Net;
using System.Text.RegularExpressions;

using TechNetNavigator.Model;

namespace TechNetNavigator.ViewModel
{
    public class RssItemViewModel : CPViewModel
    {
        private static readonly Regex regex = new Regex("\\<[^\\>]*\\>");

        /// <summary>
        /// Initializes a new instance of the RssItemViewModel class.
        /// </summary>
        public RssItemViewModel(RssItem item)
        {
            Item = item;
        }

        public string DescriptionSummary
        {
            get
            {
                return HttpUtility.HtmlDecode(regex.Replace(Item.Description, string.Empty));
            }
        }

        public RssItem Item { get; private set; }
    }
}