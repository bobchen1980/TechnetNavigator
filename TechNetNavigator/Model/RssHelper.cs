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

using TechNetNavigator.ViewModel;

namespace TechNetNavigator.Model
{
    public class RssHelper
    {
        public RssHelper()
        {
        }

        public event EventHandler LoadedCompleteEvent;
        public static int MaxCountEachPage = 50;

        public void LoadList(PanoramType listType, long sinceId, string searchTerm)
        {
            switch (listType)
            {
                case PanoramType.HotnewsType:
                    LoadHotnews(sinceId);
                    break;
                case PanoramType.BulletinType:
                    LoadBulletin(sinceId);
                    break;
                default:
                    return;
            }

        }
        private void LoadHotnews(long sinceId)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(HotnewsDownloadComplete);
            client.DownloadStringAsync(new Uri(Constants.HotnewsURLStr), UriKind.RelativeOrAbsolute);
        }
        public void HotnewsDownloadComplete(object sender, DownloadStringCompletedEventArgs e)
        {

            var client = sender as WebClient;
            if (client != null)
            {
                client.DownloadStringCompleted -= HotnewsDownloadComplete;
            }
            if (e.Error != null)
            {
                MessageBox.Show("网络信号不稳定，无法动态获取信息\n请检查网络配置，确认手机允许上网");
                return;
            }
            if (e.Result != null)
            {
                try
                {
                    var xmlElement = XElement.Parse(e.Result);

                    //Rss General Info
                    String RssTitle = (from item in xmlElement.Elements("channel") select (String)item.Element("title").Value).FirstOrDefault();
                    String RssLink = (from item in xmlElement.Elements("channel") select (String)item.Element("link").Value).FirstOrDefault();
                    String RssDescription = (from item in xmlElement.Elements("channel") select (String)item.Element("description").Value).FirstOrDefault();
                    String RssImageUrl = (from item in xmlElement.Elements("image") select (String)item.Element("url").Value).FirstOrDefault();

                    //String test = xmlElement.Elements("channel").Elements("item").Elements("title").First().Value;
                    //MessageBox.Show(test);
                    List<ItemViewModel> HotnewsList = (from item in xmlElement.Elements("channel").Elements("item")
                                                       select new ItemViewModel
                                                    {
                                                        ItemLink = (String)item.Element("link").Value,
                                                        ItemTitle = (String)item.Element("title").Value,
                                                        ItemDetails = (String)item.Element("description").Value,
                                                        ItemDate = (String)item.Element("pubDate").Value,
                                                        IsNewItem = true
                                                    }).Take(MaxCountEachPage).ToList();

                    /*/////////////////////////////////////////////////////////////////////////////
                    // Load cached file and add them but only up to 100 old items
                    var oldItems = BaseHelper.LoadSetting<List<ItemViewModel>>(Constants.HotnewsFileName);
                    if (oldItems != null)
                    {
                        var maxCount = (oldItems.Count < 20) ? oldItems.Count : 20;
                        for (var i = 0; i < maxCount; i++)
                        {
                            oldItems[i].IsNewItem = false;
                            HotnewsList.Add(oldItems[i]);
                        }
                    }*/
                    ////////////////////////////////////////////////////////////////////////////
                    BaseHelper.SaveSetting(Constants.HotnewsFileName, HotnewsList);
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

        private void LoadBulletin(long sinceId)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(BulletinDownloadComplete);
            client.DownloadStringAsync(new Uri(Constants.BulletinURLStr), UriKind.RelativeOrAbsolute);
        }
        public void BulletinDownloadComplete(object sender, DownloadStringCompletedEventArgs e)
        {

            var client = sender as WebClient;
            if (client != null)
            {
                client.DownloadStringCompleted -= BulletinDownloadComplete;
            }
            if (e.Error != null)
            {
                //MessageBox.Show("网络信号不稳定，请检查网络连接");
                return;
                //throw e.Error;    
            }
            if (e.Result != null)
            {
                try
                {
                    var xmlElement = XElement.Parse(e.Result);

                    //Rss General Info
                    String RssTitle = (from item in xmlElement.Elements("channel") select (String)item.Element("title").Value).FirstOrDefault();
                    String RssLink = (from item in xmlElement.Elements("channel") select (String)item.Element("link").Value).FirstOrDefault();
                    String RssDescription = (from item in xmlElement.Elements("channel") select (String)item.Element("description").Value).FirstOrDefault();
                    String RssImageUrl = (from item in xmlElement.Elements("image") select (String)item.Element("url").Value).FirstOrDefault();

                    //String test = xmlElement.Elements("channel").Elements("item").Elements("title").First().Value;
                    //MessageBox.Show(test);
                    List<ItemViewModel> BulletinList = (from item in xmlElement.Elements("channel").Elements("item")
                                                       select new ItemViewModel
                                                       {
                                                           ItemLink = (String)item.Element("link").Value,
                                                           ItemTitle = (String)item.Element("title").Value,
                                                           ItemDetails = BaseHelper.LocalDataTime((String)item.Element("pubDate").Value),
                                                           ItemDate = (String)item.Element("pubDate").Value,
                                                           IsNewItem = true
                                                       }).Take(MaxCountEachPage).ToList();

                    BaseHelper.SaveSetting(Constants.BulletinFileName, BulletinList);
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
    }
}
