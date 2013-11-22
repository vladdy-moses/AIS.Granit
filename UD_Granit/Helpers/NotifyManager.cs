using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UD_Granit.Helpers
{
    public class NotificationManager
    {
        public struct Notify {
            public enum NotifyType { Error, Info, Other };

            public NotifyType Type { set; get; }
            public string Message { set; get; }
            public string Class
            {
                get
                {
                    switch (Type)
                    {
                        case NotifyType.Error:
                            return "notify notify-error";
                        case NotifyType.Info:
                            return "notify notify-info";
                    }
                    return "notify";
                }
            }
        };

        public List<Notify> Notifies = new List<Notify>();
        public int Count { get { return Notifies.Count; } }
    }
}