namespace Whs.Shared.Models
{
    public static class QueType
    {
        public static class In
        {
            public const string ForCustomer = "Под клиента";
            public const string QuicklyForSale = "Срочно в продажу";
            public const string Expired = "Просрочено";
            public const string NoQue = "Очередность не указана";
        }

        public static class Out
        {
            public const string LiveQue = "Живая очередь";
            public const string Schedule = "Собрать к дате";
            public const string SelfDelivery = "Собственная доставка";
            public const string NoQue = "Очередность не указана";
        }
    }
}
