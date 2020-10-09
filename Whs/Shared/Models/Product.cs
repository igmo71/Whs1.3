using System;
using System.Text.Json.Serialization;

namespace Whs.Shared.Models
{
    public abstract class Product
    {
        public string Документ_Id { get; set; }
        public int НомерСтроки { get; set; }
        public string Документ_Name { get; set; }
        public string Номенклатура_Id { get; set; }
        public string Номенклатура_Name { get; set; }
        public string Артикул { get; set; }
        public float КоличествоФакт { get; set; }
        public float КоличествоПлан { get; set; }
        public float Вес { get; set; }
        public string Упаковка_Id { get; set; }
        public string Упаковка_Name { get; set; }
    }

    public class ProductIn : Product
    {
        public ProductDataIn Data { get; set; }
        [JsonIgnore]
        public WhsOrderIn WhsOrder { get; set; }
    }

    public class ProductOut : Product
    {
        public ProductDataOut Data { get; set; }
        [JsonIgnore]
        public WhsOrderOut WhsOrder { get; set; }
    }

    public abstract class ProductData
    {
        public Guid Id { get; set; }
        public string Документ_Id { get; set; }
        public int НомерСтроки { get; set; }       
    }

    public class ProductDataIn : ProductData
    {
        public Guid EditingCauseId { get; set; }
        public EditingCauseIn EditingCause { get; set; }
        [JsonIgnore]
        public ProductIn Product { get; set; }
    }

    public class ProductDataOut : ProductData
    {
        public Guid EditingCauseId { get; set; }
        public EditingCauseOut EditingCause { get; set; }
        [JsonIgnore]
        public ProductOut Product { get; set; }
    }
}
