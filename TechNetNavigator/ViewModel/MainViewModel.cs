using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Xml;
using System.Linq;

using TechNetNavigator.Model;

namespace TechNetNavigator.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ItemViewModel> NavigatorItems { get; set; }
        public ObservableCollection<ItemViewModel> HotnewsItems { get; set; }
        public ObservableCollection<ItemViewModel> BulletinItems { get; set; }
        
        private static readonly Object thisLock = new Object();
        private int _syncCount;
        private const int SyncItemCount = 2;

        public MainViewModel()
        {
            NavigatorItems = new ObservableCollection<ItemViewModel>();
            HotnewsItems = new ObservableCollection<ItemViewModel>();
            BulletinItems = new ObservableCollection<ItemViewModel>();
        }

        private Visibility _progressBarVisibility = Visibility.Collapsed;
        public Visibility ProgressBarVisibility
        {
            get
            {
                return _progressBarVisibility;
            }
            set
            {
                if (value != _progressBarVisibility)
                {
                    _progressBarVisibility = value;
                    NotifyPropertyChanged("ProgressBarVisibility");
                }
            }
        }

        private bool _progressBarIsIndeterminate;
        public bool ProgressBarIsIndeterminate
        {
            get
            {
                return _progressBarIsIndeterminate;
            }
            set
            {
                if (value != _progressBarIsIndeterminate)
                {
                    _progressBarIsIndeterminate = value;
                    NotifyPropertyChanged("ProgressBarIsIndeterminate");
                }
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            // Sample data; replace with real data
            LoadData(false);
            IsDataLoaded = true;
        }

        public void Refresh()
        {
            LoadData(true);
        }

        private void LoadData(bool refresh)
        {
            _syncCount = SyncItemCount;
            ProgressBarIsIndeterminate = true;
            ProgressBarVisibility = Visibility.Visible;

            LoadNavigator();
            LoadList(PanoramType.HotnewsType, refresh);
            LoadList(PanoramType.BulletinType, refresh);
        }

        private void LoadList(PanoramType listType, bool refresh)
        {
            LoadList(listType, refresh, String.Empty);
        }
        private void LoadNavigator()
        {
            StreamResourceInfo XmlResource = Application.GetResourceStream(new Uri("/TechNetNavigator;component/data/NavigatorLocalData.xml", UriKind.Relative));
            var xmlElement = XElement.Load(XmlResource.Stream);
            List<ItemViewModel> NavigatorList = (from item in xmlElement.Elements("item")
                                                 select new ItemViewModel
                                                 {
                                                     ItemTitle = (String)item.Element("title").Value,
                                                     ItemDetails = (String)item.Element("description").Value,
                                                     ItemLink = (String)item.Element("link").Value,
                                                     ItemImage = (String)item.Element("image").Value
                                                 }).ToList();
            BindList(NavigatorItems, NavigatorList);

        }
        private void LoadList(PanoramType listType, bool refresh, string searchTerm)
        {
            string fileName = null;
            ObservableCollection<ItemViewModel> parentList = null;

            switch (listType)
            {
                case PanoramType.HotnewsType:
                    fileName = Constants.HotnewsFileName;
                    parentList = HotnewsItems;
                    break;
                case PanoramType.BulletinType:
                    fileName = Constants.BulletinFileName;
                    parentList = BulletinItems;
                    break;
            }
            if (String.IsNullOrEmpty(fileName))
                return;
            // If a cached file exists, bind it first then go update unless we are refreshing
            if (!refresh)
            {
                var itemList = BaseHelper.LoadSetting<List<ItemViewModel>>(fileName);
                if (itemList != null)
                {
                    BindList(parentList, itemList);
                }
            }

            var RssHelper = new RssHelper();
            RssHelper.LoadList(listType, (parentList != null && parentList.Count > 0) ? parentList[0].ViewId : 0, searchTerm);
            RssHelper.LoadedCompleteEvent += (sender, e) =>
            {
                CheckCount();

                var list = BaseHelper.LoadSetting<List<ItemViewModel>>(fileName);
                if (list == null)
                {
                    //BaseHelper.ShowMessage("网络连接不稳定，请检查网络信号.");
                    return;
                }

                Deployment.Current.Dispatcher.BeginInvoke(() => BindList(parentList, list));
            };
        }
        private void CheckCount()
        {
            lock (thisLock)
            {
                _syncCount--;
                if (_syncCount == 0)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        ProgressBarIsIndeterminate = false;
                        ProgressBarVisibility = Visibility.Collapsed;
                    });
                }
            }
        }
        private static void BindList(ObservableCollection<ItemViewModel> parentList, IEnumerable<ItemViewModel> list)
        {
            parentList.Clear();

            foreach (var item in list)
            {
                parentList.Add(item);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}