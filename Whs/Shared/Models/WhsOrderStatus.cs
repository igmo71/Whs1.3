namespace Whs.Shared.Models
{
    public static class WhsOrderStatus
    {
        public static class In
        {
            public static string ToReceive = "К поступлению";
            public static string AtWork = "В работе";
            public static string Received = "Принят";
        }
        public static class Out
        {
            public static string Prepared = "Подготовлено";
            public static string ToCollect = "К отбору";
            public static string ToShipment = "К отгрузке";
            public static string Shipped = "К отгрузке";
        }
    }
}
