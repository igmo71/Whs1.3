using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Whs.Shared.Models.Accounts;

namespace Whs.Shared.Models
{
    public abstract class WhsOrderData
    {
        public Guid Id { get; set; }
        public string Документ_Id { get; set; }
        public string Статус { get; set; }
        public DateTime DateTime { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }

    public class WhsOrderDataIn : WhsOrderData
    {
        [JsonIgnore]
        [ForeignKey("Документ_Id")]
        public WhsOrderIn WhsOrderIn { get; set; }
    }
    public class WhsOrderDataOut : WhsOrderData
    {
        [JsonIgnore]
        [ForeignKey("Документ_Id")]
        public WhsOrderOut WhsOrderOut { get; set; }
    }
}
