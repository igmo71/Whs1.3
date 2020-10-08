using System;
using System.Collections.Generic;
using System.Text;

namespace Whs.Shared.Models
{
    public class WhsOrderParameters
    {
        public string SearchTerm { get; set; }
        public string SearchWarehouseId { get; set; }
        public string SearchDestinationId { get; set; }
        public string SearchBarcode { get; set; }
        public SearchStatus SearchStatus { get; set; }
    }

    public enum SearchStatus { Undefined, New, AtWork, ToShipment, Complete }
}
