using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Whs.Shared.Models
{
    public abstract class ProductData : Product
    {
        public ProductData() { }

        public ProductData(Product product) : this()
        {
            Документ_Id = product.Документ_Id;
            НомерСтроки = product.НомерСтроки;
            Документ_Name = product.Документ_Name;
            Номенклатура_Id = product.Номенклатура_Id;
            Номенклатура_Name = product.Номенклатура_Name;
            Артикул = product.Артикул;
            КоличествоФакт = product.КоличествоФакт;
            КоличествоПлан = product.КоличествоПлан;
            Вес = product.Вес;
            Упаковка_Id = product.Упаковка_Id;
            Упаковка_Name = product.Упаковка_Name;
            EditingCauseId = product.EditingCauseId;
        }

        [Key]
        public Guid Id { get; set; }
    }

    public class ProductDataIn : ProductData
    {
        public ProductDataIn() : base() { }
        public ProductDataIn(Product product) : base(product) { }

        [JsonIgnore]
        [ForeignKey("Документ_Id")]
        public WhsOrderIn WhsOrder { get; set; }

        [JsonIgnore]
        [ForeignKey("EditingCauseId")]
        public EditingCauseIn EditingCause { get; set; }
    }

    public class ProductDataOut : ProductData
    {
        public ProductDataOut() : base() { }
        public ProductDataOut(Product product) : base(product) { }

        [JsonIgnore]
        [ForeignKey("Документ_Id")]
        public WhsOrderOut WhsOrder { get; set; }

        [JsonIgnore]
        [ForeignKey("EditingCauseId")]
        public EditingCauseOut EditingCause { get; set; }
    }
}
