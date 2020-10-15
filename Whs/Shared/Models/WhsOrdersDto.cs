using System.Collections.Generic;

namespace Whs.Shared.Models
{
    public class WhsOrdersDto
    {
        public string SingleId { get; set; }
        public string MngrOrderName { get; set; }
        public string TotalWeight { get; set; }
        public string TotalCount { get; set; }
    }

    public class WhsOrdersDtoIn : WhsOrdersDto
    {
        public Dictionary<string, WhsOrderIn[]> Items { get; set; }
    }

    public class WhsOrdersDtoOut : WhsOrdersDto
    {
        public Dictionary<string, WhsOrderOut[]> Items { get; set; }
        public Destination[] Destinations { get; set; }
    }
}
