using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Shapes;

namespace TechNetNavigator.Model
{
    public class Constants
    {
        public static string HotnewsURLStr = "http://www.microsoft.com/China/TechNet/rss/homepage/new.xml";
        public static string BulletinURLStr = "http://www.microsoft.com/china/technet/rss/security/bulletins.xml";
        public static string AdvisoryURLStr = "http://www.microsoft.com/china/technet/rss/security/advisory1.xml";
        public static string HotnewsFileName = "HotnewsFileName";
        public static string BulletinFileName = "BulletinFileName";
        public static string AdvisoryFileName = "AdvisoryFileName";

        public static string TechNetBlogFileName = "TechNetBlogFile";
        public static string UserInforFileName = "UserInforFile";
        public static string StatusesFileName = "StatusesFile";
        public static string MentionsFileName = "MentionsFile";
        public static string DirectMessagesFileName = "MessagesFile"; 
        public static string DetailPageFileName = "DetailPageFile";
        public static string BlogDetailFileName = "BlogDetailFileName";

        public static string SearchStr = "Microsoft TechNet 中国";

        public static string AppKey = "2863206723";   //Provided by Chen Xiaoliang @ CAS
        public static string BlogStatusesURLStr = "http://api.t.sina.com.cn/statuses/user_timeline.xml?source=2863206723&user_id=1655262755";

    }

    public enum PanoramType { HotnewsType, BulletinType }

    public enum MicroBlogType { TechNets, UserInfor, Statuses, Mentions, DirectMessages, Favorites }
}
