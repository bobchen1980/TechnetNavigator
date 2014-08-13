using System;
using System.Windows;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;

using CPVanity.Model;

namespace CPVanity.ViewModel
{
    public class RssFeedViewModel : CPViewModel
    {
        private RssFeed _feed;
        private Uri _uri;

        /// <summary>
        /// Initializes a new instance of the RssViewModel class.
        /// </summary>
        public RssFeedViewModel(Uri uri)
        {
            _uri = uri;
        }

        public DataTemplate ItemDataTemplate
        {
            get
            {
                return App.Current.Resources["RssFeedTemplate"] as DataTemplate;
            }
        }

        private ObservableCollection<RssItemViewModel> _items = new ObservableCollection<RssItemViewModel>();
        public ObservableCollection<RssItemViewModel> Items
        {
            get
            {
                if (_feed == null)
                {
                    Working = true;

                    _feed = new RssFeed(_uri);
                    _feed.ChannelChanged += new EventHandler(_feed_ChannelChanged);
                    _feed.Load();
                }

                return _items;
            }
        }

        void _feed_ChannelChanged(object sender, EventArgs e)
        {
            if (_feed.Channel != null)
            {
                foreach (var item in _feed.Channel.Items)
                    Items.Add(new RssItemViewModel(item));
            }

            Working = false;
        }

        public override void Refresh()
        {
            if (!Working && _feed != null)
            {
                _items.Clear();
                Working = true;
                _feed.Load();
            }
            base.Refresh();
        }
    }
}