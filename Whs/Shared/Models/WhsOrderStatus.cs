namespace Whs.Shared.Models
{
    public static class WhsOrderStatus
    {
        public static class In
        {
            public const string ToReceive = "К поступлению";
            public const string AtWork = "В работе";
            public const string Received = "Принят";
        }
        public static class Out
        {
            public const string Prepared = "Подготовлено";
            public const string ToCollect = "К отбору";
            public const string ToShipment = "К отгрузке";
            public const string Shipped = "Отгружен";
        }
    }
}
