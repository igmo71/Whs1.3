using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Whs.Shared.Models
{
    public class Warehouses
    {
        [JsonPropertyName("value")]
        public List<Warehouse> List { get; set; }
    }

    public class Warehouse
    {
        [Key]
        [JsonPropertyName("Ref_Key")]
        public string Id { get; set; }

        [JsonPropertyName("Description")]
        public string Name { get; set; }

        public List<ApplicationUser> Users { get; set; }
    }
}