using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UD_Granit.Helpers
{
    // Задаёт механизм уведомлений
    public class NotificationManager
    {
        // Описывает уведомление
        public struct Notify
        {
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

    // Помогает отоюражать уведомления в представлениях
    public static class NotificationHelper
    {
        // Добавляет метод по добавлению уведомления
        public static NotificationManager NotificationAdd(this ViewDataDictionary viewData, NotificationManager.Notify notify)
        {
            NotificationManager nManager = (viewData["UserNotification"] != null) ? (viewData["UserNotification"] as NotificationManager) : new NotificationManager();
            nManager.Notifies.Add(notify);
            viewData["UserNotification"] = nManager;

            return nManager;
        }

        // Выдаёт управляющий уведомлениями экземпляр класса
        public static NotificationManager Notifications(this HtmlHelper helper)
        {
            return (helper.ViewData["UserNotification"] as NotificationManager);
        }
    }
}