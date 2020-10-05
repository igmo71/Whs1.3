using System.Text.Json.Serialization;

namespace Whs.Shared.Models
{
    public abstract class MngOrder
    {
        public string Документ_Id { get; set; }
        public string Документ_Name { get; set; }
        public string Распоряжение_Id { get; set; }
        public string Распоряжение_Name { get; set; }
    }

    public class MngOrderIn : MngOrder
    {
        [JsonIgnore]
        public WhsOrderIn WhsOrder { get; set; }
    }

    public class MngOrderOut : MngOrder
    {
        [JsonIgnore]
        public WhsOrderOut WhsOrder { get; set; }
    }

}
