using System;
using System.Collections.Generic;
using System.Text;

namespace Whs.Shared.Models
{
    public class WhsOrderSettings
    {
        public const string WhsOrder = "WhsOrder";

        public int OrdersPerPage { get; set; }
        public MatchingStatus MatchingStatusOut { get; set; }
        public MatchingStatus MatchingStatusIn { get; set; }
    }

    public class MatchingStatus
    {
        public string[] Show { get; set; }
        public string AtWork { get; set; }
        public string ToShipment { get; set; }
        public string Complete { get; set; }
    }
}
