﻿using System;
using System.Text.Json.Serialization;

namespace Whs.Shared.Models
{
    public abstract class Product
    {
        public string Документ_Id { get; set; }
        public string Документ_Name { get; set; }
        public int НомерСтроки { get; set; }
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
        [JsonIgnore]
        public WhsOrderIn WhsOrder { get; set; }
    }

    public class ProductOut : Product
    {
        [JsonIgnore]
        public WhsOrderOut WhsOrder { get; set; }
    }

    public class ProductData
    {
        public string Документ_Id { get; set; }
        public int НомерСтроки { get; set; }
        public Product Product { get; set; }

        public Guid EditingCauseId { get; set; }
        public EditingCause EditingCause { get; set; }
    }
}
