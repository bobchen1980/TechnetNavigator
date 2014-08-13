using System;
using System.ComponentModel;

namespace TechNetNavigator.ViewModel
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        private string itemTitle;
        public string ItemTitle
        {
            get { return itemTitle; }
            set
            {
                if (value != itemTitle)
                {
                    itemTitle = value;
                    NotifyPropertyChanged("ItemTitle");
                }
            }
        }

        private string itemDetails;
        public string ItemDetails
        {
            get { return itemDetails; }
            set
            {
                if (value != itemDetails)
                {
                    itemDetails = value;
                    NotifyPropertyChanged("ItemDetails");
                }
            }
        }

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
                    NotifyPropertyChanged("UserName");
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
                    NotifyPropertyChanged("DisplayUserName");
                }
            }
        }

        public bool IsNewItem { get; set; }
        public long ViewId { get; set; }

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
