using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace TechNetNavigator.BlogModel
{
    public class BlogItemModel : INotifyPropertyChanged
    {
        private string itemLink;
        public string ItemLink
        {
            get { return itemLink; }
            set
            {
                if (value != itemLink)
                {
                    itemLink = value;
                    NotifyPropertyChanged("ItemLink");
                }
            }
        }
 
        private string _itemText;
        public string ItemText
        {
            get { return _itemText; }
            set
            {
                if (value != _itemText)
                {
                    _itemText = value;
                    NotifyPropertyChanged("ItemText");
                }
            }
        }

        private string _image;
        public string ItemImage
        {
            get
            {
                return _image;
            }
            set
            {
                if (value != _image)
                {
                    _image = value;
                    NotifyPropertyChanged("ItemImage");
                }
            }
        }
        private string _createdDate;
        public string ItemDate
        {
            get
            {
                return _createdDate;
            }
            set
            {
                if (value != _createdDate)
                {
                    _createdDate = value;
                    NotifyPropertyChanged("ItemDate");
                }
            }
        }

        private string _userName;
        public string ItemUser
        {
            get
            {
                return _userName;
            }
            set
            {
                if (value != _userName)
                {
                    _userName = value;
                    NotifyPropertyChanged("ItemUser");
                }
            }
        }

        private string _displayUserName;
        public string ItemDisplayUser
        {
            get
            {
                return _displayUserName;
            }
            set
            {
                if (value != _displayUserName)
                {
                    _displayUserName = value;
                    NotifyPropertyChanged("ItemDisplayUser");
                }
            }
        }

        private string _thumbnail;
        public string Itemthumbnail
        {
            get
            {
                return _thumbnail;
            }
            set
            {
                if (value != _thumbnail)
                {
                    _thumbnail = value;
                    NotifyPropertyChanged("Itemthumbnail");
                }
            }
        }

        private string _original_pic;
        public string ItemOriginalpic
        {
            get
            {
                return _original_pic;
            }
            set
            {
                if (value != _original_pic)
                {
                    _original_pic = value;
                    NotifyPropertyChanged("ItemOriginalpic");
                }
            }
        }

        private string _location;
        public string ItemLocation
        {
            get
            {
                return _location;
            }
            set
            {
                if (value != _location)
                {
                    _location = value;
                    NotifyPropertyChanged("ItemLocation");
                }
            }
        }

        private string _description;
        public string ItemDetail
        {
            get
            {
                return _description;
            }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    NotifyPropertyChanged("ItemDetail");
                }
            }
        }

        private string _followers_count;
        public string ItemFollowers
        {
            get
            {
                return _followers_count;
            }
            set
            {
                if (value != _followers_count)
                {
                    _followers_count = value;
                    NotifyPropertyChanged("ItemFollowers");
                }
            }
        }

        private string _friends_count;
        public string ItemFriends
        {
            get
            {
                return _friends_count;
            }
            set
            {
                if (value != _friends_count)
                {
                    _friends_count = value;
                    NotifyPropertyChanged("ItemFriends");
                }
            }
        }
        
        private string _statuses_count;
        public string ItemBlogCount
        {
            get
            {
                return _statuses_count;
            }
            set
            {
                if (value != _statuses_count)
                {
                    _statuses_count = value;
                    NotifyPropertyChanged("ItemBlogCount");
                }
            }
        }
/// <summary>
/// ///////////////////////////Reteeted definition
/// </summary>
        private string retweeted_itemMergedText;
        public string ReItemMergedText
        {
            get { return retweeted_itemMergedText; }
            set
            {
                if (value != retweeted_itemMergedText)
                {
                    retweeted_itemMergedText = value;
                    NotifyPropertyChanged("ReItemMergedText");
                }
            }
        }

        private string retweeted_itemText;
        public string ReItemText
        {
            get { return retweeted_itemText; }
            set
            {
                if (value != retweeted_itemText)
                {
                    retweeted_itemText = value;
                    NotifyPropertyChanged("retweeted_ItemText");
                }
            }
        }

        private string retweeted_image;
        public string ReItemImage
        {
            get
            {
                return retweeted_image;
            }
            set
            {
                if (value != retweeted_image)
                {
                    retweeted_image = value;
                    NotifyPropertyChanged("ReItemImage");
                }
            }
        }

        private string _reoriginal_pic;
        public string ReItemOriginalpic
        {
            get
            {
                return _reoriginal_pic;
            }
            set
            {
                if (value != _reoriginal_pic)
                {
                    _reoriginal_pic = value;
                    NotifyPropertyChanged("ReItemOriginalpic");
                }
            }
        }

        private string retweeted_createdDate;
        public string ReItemDate
        {
            get
            {
                return retweeted_createdDate;
            }
            set
            {
                if (value != retweeted_createdDate)
                {
                    retweeted_createdDate = value;
                    NotifyPropertyChanged("ReItemDate");
                }
            }
        }

        private string retweeted_userName;
        public string ReItemUser
        {
            get
            {
                return retweeted_userName;
            }
            set
            {
                if (value != retweeted_userName)
                {
                    retweeted_userName = value;
                    NotifyPropertyChanged("ReItemUser");
                }
            }
        }

        private string retweeted_displayUserName;
        public string ReItemDisplayUser
        {
            get
            {
                return retweeted_displayUserName;
            }
            set
            {
                if (value != retweeted_displayUserName)
                {
                    retweeted_displayUserName = value;
                    NotifyPropertyChanged("ReItemDisplayUser");
                }
            }
        }

        private string retweeted_thumbnail;
        public string ReItemthumbnail
        {
            get
            {
                return retweeted_thumbnail;
            }
            set
            {
                if (value != retweeted_thumbnail)
                {
                    retweeted_thumbnail = value;
                    NotifyPropertyChanged("ReItemthumbnail");
                }
            }
        }


        public SolidColorBrush ItemTitleColor
        {
            get
            {
                return IsNewBlog ? (SolidColorBrush)Application.Current.Resources["PhoneForegroundBrush"]
                               : (SolidColorBrush)Application.Current.Resources["PhoneBackgroundBrush"];
            }
        }

        public bool IsNewBlog { get; set; }
        public long BlogId { get; set; }

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
