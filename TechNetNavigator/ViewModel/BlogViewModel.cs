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

namespace TechNetNavigator.BlogModel
{
    public class BlogViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<BlogItemModel> TechNetBlogs { get; set; }
        public ObservableCollection<BlogItemModel> UserInforItems { get; set; }
        
        
        private static readonly Object thisLock = new Object();
        private int _syncCount;
        private const int SyncItemCount = 1;

        public BlogViewModel()
        {
            TechNetBlogs = new ObservableCollection<BlogItemModel>();
            UserInforItems = new ObservableCollection<BlogItemModel>(); 
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

            LoadList(MicroBlogType.TechNets, refresh);

            LoadUserInfo();

        }

        private void LoadUserInfo()
        {
            var itemList = BaseHelper.LoadSetting<List<BlogItemModel>>(Constants.TechNetBlogFileName);
            if (itemList != null)
            {
                UserInforItems.Clear();
                UserInforItems.Add(itemList.FirstOrDefault());
            }
        }

        private void LoadList(MicroBlogType listType, bool refresh)
        {
            LoadList(listType, refresh, String.Empty);
        }

        private void LoadList(MicroBlogType listType, bool refresh, string searchTerm)
        {
            string fileName = null;
            ObservableCollection<BlogItemModel> parentList = null;

            switch (listType)
            {
                case MicroBlogType.TechNets:
                    fileName = Constants.TechNetBlogFileName;
                    parentList = TechNetBlogs;
                    break;
            }
            if (String.IsNullOrEmpty(fileName))
                return;
            // If a cached file exists, bind it first then go update unless we are refreshing
            if (!refresh)
            {
                var itemList = BaseHelper.LoadSetting<List<BlogItemModel>>(fileName);
                if (itemList != null)
                {
                    BindList(parentList, itemList);
                }
            }

            var BlogHelper = new BlogHelper();
            BlogHelper.LoadList(listType, (parentList != null && parentList.Count > 0) ? parentList[0].BlogId : 0, searchTerm);
            BlogHelper.LoadedCompleteEvent += (sender, e) =>
            {
                CheckCount();

                var list = BaseHelper.LoadSetting<List<BlogItemModel>>(fileName);
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

        private static void BindList(ObservableCollection<BlogItemModel> parentList, IEnumerable<BlogItemModel> list)
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
