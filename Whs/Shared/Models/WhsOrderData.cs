using System;
using System.Text.Json.Serialization;
using Whs.Shared.Models.Accounts;

namespace Whs.Shared.Models
{
    public abstract class WhsOrderData
    {
        public Guid Id { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public DateTime StatusSwitchDate { get; set; }
    }

    public class WhsOrderDataIn : WhsOrderData
    {
        public string WhsOrderInId { get; set; }
        [JsonIgnore]
        public WhsOrderIn WhsOrderIn { get; set; }
    }
    public class WhsOrderDataOut : WhsOrderData
    {
        public string WhsOrderOutId { get; set; }
        [JsonIgnore]
        public WhsOrderOut WhsOrderOut { get; set; }
    }
}
