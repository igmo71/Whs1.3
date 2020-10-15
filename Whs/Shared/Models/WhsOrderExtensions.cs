using System;
using System.Linq;

namespace Whs.Shared.Models
{
    public static class WhsOrderExtensions
    {
        public static IQueryable<WhsOrderOut> Search(this IQueryable<WhsOrderOut> query, WhsOrderParameters parameters)
        {
            if (!string.IsNullOrWhiteSpace(parameters.SearchStatus))
            {
                switch (parameters.SearchStatus)
                {
                    case "Подготовлено":
                        query = query.Where(e => e.Статус == WhsOrderStatus.Out.Prepared)
                            .OrderByDescending(e => e.ВесовойКоэффициент).ThenBy(e => e.СрокВыполнения);
                        break;
                    case "К отбору":
                        query = query.Where(e => e.Статус == WhsOrderStatus.Out.ToCollect)
                            .OrderByDescending(e => e.ВесовойКоэффициент).ThenBy(e => e.СрокВыполнения);
                        break;
                    case "К отгрузке":
                        query = query.Where(e => e.Статус == WhsOrderStatus.Out.ToShipment)
                            .OrderBy(e => e.Data.Where(e => e.Статус == "К отгрузке").OrderByDescending(d => d.DateTime).FirstOrDefault().DateTime);
                        break;
                    case "Отгружен":
                        query = query.Where(e => e.Статус == WhsOrderStatus.Out.Shipped)
                            .OrderBy(e => e.Data.Where(e => e.Статус == "Отгружен").OrderByDescending(d => d.DateTime).FirstOrDefault().DateTime);
                        break;
                    default:
                        break;
                }
            }

            if (!(string.IsNullOrWhiteSpace(parameters.SearchWarehouseId) || parameters.SearchWarehouseId == Guid.Empty.ToString()))
                query = query.Where(e => e.Склад_Id == parameters.SearchWarehouseId);

            if (!(string.IsNullOrWhiteSpace(parameters.SearchDestinationId) || parameters.SearchDestinationId == "0"))
                query = query.Where(e => e.НаправлениеДоставки_Id == parameters.SearchDestinationId);

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var lowerCaseSearchTerm = parameters.SearchTerm.Trim().ToLower();
                query = query.Where(e =>
                    e.Номер.ToLower().Contains(lowerCaseSearchTerm) ||
                    e.НомерОчереди.ToLower().Contains(lowerCaseSearchTerm) ||
                    e.ОтправительПолучатель_Name.ToLower().Contains(lowerCaseSearchTerm));
            }
            return query;
        }

        public static IQueryable<WhsOrderIn> Search(this IQueryable<WhsOrderIn> query, WhsOrderParameters parameters)
        {
            if (!string.IsNullOrWhiteSpace(parameters.SearchStatus))
            {
                switch (parameters.SearchStatus)
                {
                    case "К поступлению":
                        query = query.Where(e => e.Статус == WhsOrderStatus.In.ToReceive)
                            .OrderByDescending(e => e.ВесовойКоэффициент).ThenBy(e => e.СрокВыполнения);
                        break;
                    case "В работе":
                        query = query.Where(e => e.Статус == WhsOrderStatus.In.AtWork)
                            .OrderByDescending(e => e.ВесовойКоэффициент).ThenBy(e => e.СрокВыполнения);
                        break;
                    case "Принят":
                        query = query.Where(e => e.Статус == WhsOrderStatus.In.Received)
                            .OrderBy(e => e.Data.Where(e => e.Статус == "Принят").OrderByDescending(d => d.DateTime).FirstOrDefault().DateTime); 
                        break;
                    default:
                        break;
                }
            }

            if (!(string.IsNullOrWhiteSpace(parameters.SearchWarehouseId) || parameters.SearchWarehouseId == Guid.Empty.ToString()))
                query = query.Where(e => e.Склад_Id == parameters.SearchWarehouseId);

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var lowerCaseSearchTerm = parameters.SearchTerm.Trim().ToLower();
                query = query.Where(e =>
                    e.Номер.ToLower().Contains(lowerCaseSearchTerm) ||
                    e.ОтправительПолучатель_Name.ToLower().Contains(lowerCaseSearchTerm));
            }
            return query;
        }
    }
}
