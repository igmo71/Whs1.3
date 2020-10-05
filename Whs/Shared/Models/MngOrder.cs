using System.Text.Json.Serialization;

namespace Whs.Shared.Models
{
    public abstract class MngrOrder
    {
        public string Документ_Id { get; set; }
        public string Документ_Name { get; set; }
        public string Распоряжение_Id { get; set; }
        public string Распоряжение_Name { get; set; }
    }

    public class MngrOrderIn : MngrOrder
    {
        [JsonIgnore]
        public WhsOrderIn WhsOrder { get; set; }
    }

    public class MngrOrderOut : MngrOrder
    {
        [JsonIgnore]
        public WhsOrderOut WhsOrder { get; set; }
    }

}
