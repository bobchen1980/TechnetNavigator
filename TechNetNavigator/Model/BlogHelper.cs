using System;
using System.Net;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Diagnostics;
using System.Xml.Linq;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

using TechNetNavigator.BlogModel;

namespace TechNetNavigator.Model
{
    public class BlogHelper
    {
        public event EventHandler LoadedCompleteEvent;

        public void LoadList(MicroBlogType listType, long sinceId, string searchTerm)
        {
            switch (listType)
            {
                case MicroBlogType.TechNets:
                    LoadNewTechNets(sinceId);
                    break;
                default:
                    return;
            }

        }

        private void LoadNewTechNets(long sinceId)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(TechNetBlogDownloadComplete);
            client.DownloadStringAsync(new Uri(Constants.BlogStatusesURLStr), UriKind.RelativeOrAbsolute);
        }
        public void TechNetBlogDownloadComplete(object sender, DownloadStringCompletedEventArgs e)
        {

            var client = sender as WebClient;
            if (client != null)
            {
                client.DownloadStringCompleted -= TechNetBlogDownloadComplete;
            }
            if (e.Error != null)
            {
                return;
                //throw e.Error;
            }
            if (e.Error == null && e.Result != null)
            {
                try
                {
                    var xmlElement = XElement.Parse(e.Result);

                    //String test = xmlElement.Elements("channel").Elements("item").Elements("title").First().Value;
                    //MessageBox.Show(test);
                    List<BlogItemModel> TechNetList = (from item in xmlElement.Elements("status")
                                                        select new BlogItemModel
                                                        {
                                                            ItemUser = GetChildElementValue(item, "user", "screen_name"),
                                                            ItemLink = GetChildElementValue(item, "user", "url"),
                                                            ItemDisplayUser = GetChildElementValue(item, "user", "name"),
                                                            ItemText = (string)item.Element("text"),
                                                            ItemDate = GetCreatedDate((string)item.Element("created_at")),
                                                            ItemImage = GetChildElementValue(item, "user", "profile_image_url"),
                                                            ItemLocation = GetChildElementValue(item, "user", "location"),
                                                            ItemDetail = GetChildElementValue(item, "user", "description"),
                                                            ItemFollowers = "粉丝：" + GetChildElementValue(item, "user", "followers_count"),
                                                            ItemFriends = "关注：" + GetChildElementValue(item, "user", "friends_count"),
                                                            ItemBlogCount = "微博：" + GetChildElementValue(item, "user", "statuses_count"),
                                                            Itemthumbnail = (string)item.Element("thumbnail_pic"),
                                                            ItemOriginalpic = (string)item.Element("original_pic"),
                                                            ReItemUser = GetThirdElementValue(item, "retweeted_status", "user", "text"),
                                                            ReItemDisplayUser = GetThirdElementValue(item, "retweeted_status", "user", "name"),
                                                            ReItemText = GetChildElementValue(item, "retweeted_status", "text"),
                                                            ReItemMergedText = GetReTweetedText(GetThirdElementValue(item, "retweeted_status", "user", "name"), GetChildElementValue(item, "retweeted_status", "text")),
                                                            ReItemDate = GetCreatedDate(GetChildElementValue(item, "retweeted_status", "created_at")),
                                                            ReItemImage = GetChildElementValue(item, "retweeted_status", "thumbnail_pic"),
                                                            ReItemOriginalpic = GetChildElementValue(item, "retweeted_status", "original_pic"),
                                                            BlogId = (long)item.Element("id"),
                                                            IsNewBlog = true,
                                                        }).ToList();

                    BaseHelper.SaveSetting(Constants.TechNetBlogFileName, TechNetList);

                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message); 
                    Debug.WriteLine(ex.Message);
                }

                if (LoadedCompleteEvent != null)
                    LoadedCompleteEvent(this, EventArgs.Empty);
            }
        }


        private static string GetChildElementValue(XElement itemElement, string parentElement, string childElement)
        {
            var userElement = itemElement.Element(parentElement);
            if (userElement == null)
                return String.Empty;

            var iteem = userElement.Element(childElement);
            if (iteem == null)
                return String.Empty;

            return iteem.Value;
        }

        private static string GetThirdElementValue(XElement itemElement, string parentElement, string childElement, string thirdElement)
        {
            var userElement = itemElement.Element(parentElement);
            if (userElement == null)
                return String.Empty;

            var second = userElement.Element(childElement);
            if (second == null)
                return String.Empty;

            var iteem = second.Element(thirdElement);
            if (iteem == null)
                return String.Empty;

            return iteem.Value;
        }

        private static string GetCreatedDate(string createdDate)
        {
            if (String.IsNullOrEmpty(createdDate))
                return String.Empty;

             DateTime date = BaseHelper.ParseDateTime(createdDate);
             //Debug.WriteLine(date);
             return date.ToString();

        }

        private static string GetReTweetedText(string tweeted_displayuser,string tweeted_text)
        {
            var ret = String.IsNullOrEmpty(tweeted_displayuser) ? String.Empty : "@" + tweeted_displayuser + ": " + tweeted_text;
            return ret;
        } 

    }
}
