using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
        private readonly int maxChars = 26;
        [NotMapped]
        public string СрокВыполненияString =>
            (СрокВыполнения == null || СрокВыполнения == DateTime.Parse("01.01.0001 0:00:00")) ? string.Empty : СрокВыполнения.ToString();
        [NotMapped]
        public string КомментарийString =>
            (string.IsNullOrEmpty(Комментарий) || Комментарий.Length < maxChars) ? Комментарий : $"{Комментарий.Substring(0, maxChars)}...";
        [NotMapped]
        public string Склад_NameString =>
            (string.IsNullOrEmpty(Склад_Name) || Склад_Name.Length < maxChars) ? Склад_Name : $"{Склад_Name.Substring(0, maxChars)}...";
        [NotMapped]
        public string ОтправительПолучатель_NameString =>
            (string.IsNullOrEmpty(ОтправительПолучатель_Name) || ОтправительПолучатель_Name.Length < maxChars) ? ОтправительПолучатель_Name : $"{ОтправительПолучатель_Name.Substring(0, maxChars)}...";

        [NotMapped]
        public string TimeUpString
        {
            get
            {
                TimeSpan timeSpan;
                DateTime now = DateTime.Now;
                string result;
                if (СрокВыполнения > now)
                {
                    timeSpan = СрокВыполнения - now;
                    result = "Осталось ";
                }
                else
                {
                    timeSpan = now - СрокВыполнения;
                    result = "Истек ";
                }
                if (timeSpan.Days > 0)
                {
                    int days = timeSpan.Days;
                    result += $"{timeSpan.Days} дн. ";
                    timeSpan -= new TimeSpan(days, 0, 0, 0);
                }
                if (timeSpan.Hours > 0)
                {
                    int hours = timeSpan.Hours;
                    result += $"{timeSpan.Hours} ч. ";
                    timeSpan -= new TimeSpan(0, hours, 0, 0);
                }
                if (timeSpan.Minutes > 0)
                {
                    int minutes = timeSpan.Minutes;
                    result += $"{timeSpan.Minutes} мин.";
                }
                if (now >= СрокВыполнения)
                {
                    result += " назад";
                }
                return result;
            }
        }
    }

    public class WhsOrderIn : WhsOrder
    {
        public List<MngrOrderIn> Распоряжения { get; set; }
        public List<ProductIn> Товары { get; set; }
        public List<WhsOrderDataIn> Data { get; set; }
        public List<ProductDataIn> ProductsData { get; set; }

        [NotMapped]
        public WhsOrderDataIn LastData => Data?.OrderByDescending(e => e.DateTime).FirstOrDefault();
    }

    public class WhsOrderOut : WhsOrder
    {
        public List<MngrOrderOut> Распоряжения { get; set; }
        public List<ProductOut> Товары { get; set; }
        public List<WhsOrderDataOut> Data { get; set; }
        public List<ProductDataOut> ProductsData { get; set; }

        public string НомерОчереди { get; set; }
        public string НаправлениеДоставки_Id { get; set; }
        public string НаправлениеДоставки_Name { get; set; }
        public string НаправлениеДоставкиРодитель_Id { get; set; }
        public string НаправлениеДоставкиРодитель_Name { get; set; }
        public bool Оплачено { get; set; }
        public bool Отгрузить { get; set; }


        [NotMapped]
        public WhsOrderDataOut LastData => Data?.OrderByDescending(e => e.DateTime).FirstOrDefault();
    }
}
