using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Whs.Shared.Models.Accounts;

namespace Whs.Shared.Models
{
    public abstract class WhsOrderData
    {
        [Key]
        public Guid Id { get; set; }
        public string Документ_Id { get; set; }
        public string Статус { get; set; }
        public DateTime DateTime { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }

    public class WhsOrderDataIn : WhsOrderData
    {
        [ForeignKey("Документ_Id")]
        public WhsOrderIn WhsOrder { get; set; }
    }
    public class WhsOrderDataOut : WhsOrderData
    {
        [ForeignKey("Документ_Id")]
        public WhsOrderOut WhsOrder { get; set; }
    }
}
