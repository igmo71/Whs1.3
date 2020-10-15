using System;
using System.Collections.Generic;
using System.Text;

namespace Whs.Shared.Models
{
    public static class QueType
    {
        public static class In
        {
            public static string ForCustomer = "Под клиента";
            public static string QuicklyForSale = "Срочно в продажу";
            public static string Expired = "Просрочено";
            public static string NoQue = "Очередность не указана";
        }

        public static class Out
        {
            public static string LiveQue = "Живая очередь";
            public static string Schedule = "Собрать к дате";
            public static string SelfDelivery = "Собственная доставка";
            public static string NoQue = "Очередность не указана";
        }
    }
}
