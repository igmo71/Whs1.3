namespace Whs.Shared.Models
{
    public class WhsOrderDto
    {
        public string BarcodeBase64 { get; set; }
        public string UserName { get; set; }
    }

    public class WhsOrderDtoIn : WhsOrderDto
    {
        public WhsOrderIn Item { get; set; }
    }

    public class WhsOrderDtoOut : WhsOrderDto
    {
        public WhsOrderOut Item { get; set; }
    }
}
