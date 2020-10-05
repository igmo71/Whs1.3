namespace Whs.Shared.Models
{
    class WhsOrderDto
    {
        public string BarcodeBase64 { get; set; }
    }

    class WhsOrderDtoOut : WhsOrderDto
    {
        public WhsOrderOut Item { get; set; }
    }

    class WhsOrderDtoIn : WhsOrderDto
    {
        public WhsOrderIn Item { get; set; }
    }

}
