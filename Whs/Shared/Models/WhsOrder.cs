using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Whs.Shared.Models
{
    public abstract class WhsOrder
    {
        public string Документ_Id { get; set; }
        public string Документ_Name { get; set; }
        public string Номер { get; set; }
        public DateTime Дата { get; set; }
        public bool Проведен { get; set; }
        public string Статус { get; set; }
        public int КоличествоСтрок { get; set; }
        public float Вес { get; set; }
        public DateTime СрокВыполнения { get; set; }
        public string ТипОчереди { get; set; }
        public int ВесовойКоэффициент { get; set; }
        public string Комментарий { get; set; }
        public bool ЭтоПеремещение { get; set; }
        public string Склад_Id { get; set; }
        public string Склад_Name { get; set; }
        public string Ответственный_Id { get; set; }
        public string Ответственный_Name { get; set; }
        public string ОтправительПолучатель_Id { get; set; }
        public string ОтправительПолучатель_Name { get; set; }



        [NotMapped]
        public string СрокВыполненияString =>
            (СрокВыполнения == null || СрокВыполнения == DateTime.Parse("01.01.0001 0:00:00")) ? string.Empty : СрокВыполнения.ToString();
        [NotMapped]
        public string КомментарийString =>
            (string.IsNullOrEmpty(Комментарий) || Комментарий.Length < 33) ? Комментарий : $"{Комментарий.Substring(0, 33)}...";
        [NotMapped]
        public string Склад_NameString => 
            (string.IsNullOrEmpty(Склад_Name) || Склад_Name.Length < 33) ? Склад_Name : $"{Склад_Name.Substring(0, 33)}...";
        [NotMapped]
        public string ОтправительПолучатель_NameString =>
            (string.IsNullOrEmpty(ОтправительПолучатель_Name) || ОтправительПолучатель_Name.Length < 33) ? ОтправительПолучатель_Name : $"{ОтправительПолучатель_Name.Substring(0, 33)}...";
       
    }

    public class WhsOrderIn : WhsOrder
    {
        public List<MngrOrderIn> Распоряжения { get; set; }
        public List<ProductIn> Товары { get; set; }
        public List<WhsOrderDataIn> Data { get; set; }
    }

    public class WhsOrderOut : WhsOrder
    {
        public List<MngrOrderOut> Распоряжения { get; set; }
        public List<ProductOut> Товары { get; set; }
        public List<WhsOrderDataOut> Data { get; set; }

        public string НомерОчереди { get; set; }
        public string НаправлениеДоставки_Id { get; set; }
        public string НаправлениеДоставки_Name { get; set; }
        public bool Оплачено { get; set; }
    }
}
