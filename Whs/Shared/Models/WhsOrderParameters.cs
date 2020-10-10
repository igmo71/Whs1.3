namespace Whs.Shared.Models
{
    public class WhsOrderParameters
    {
        public string SearchTerm { get; set; }
        public string SearchWarehouseId { get; set; }
        public string SearchDestinationId { get; set; }
        public string SearchBarcode { get; set; }
        public string SearchStatus { get; set; }
    }
}
