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
                    case WhsOrderStatus.Out.Prepared:
                        query = query.Where(e => e.Статус == WhsOrderStatus.Out.Prepared)
                            .OrderByDescending(e => e.ВесовойКоэффициент).ThenBy(e => e.СрокВыполнения);
                        break;
                    case WhsOrderStatus.Out.ToCollect:
                        query = query.Where(e => e.Статус == WhsOrderStatus.Out.ToCollect)
                            .OrderByDescending(e => e.ВесовойКоэффициент).ThenBy(e => e.СрокВыполнения);
                        break;
                    case WhsOrderStatus.Out.ToShipment:
                        query = query.Where(e => e.Статус == WhsOrderStatus.Out.ToShipment)
                            .OrderBy(e => e.Data.Where(e => e.Статус == WhsOrderStatus.Out.ToShipment).OrderByDescending(d => d.DateTime).FirstOrDefault().DateTime);
                        break;
                    case WhsOrderStatus.Out.Shipped:
                        query = query.Where(e => e.Статус == WhsOrderStatus.Out.Shipped)
                            .OrderBy(e => e.Data.Where(d => d.Статус == WhsOrderStatus.Out.Shipped).OrderByDescending(d => d.DateTime).FirstOrDefault().DateTime);
                        break;
                    default:
                        break;
                }
            }

            if (!(string.IsNullOrWhiteSpace(parameters.SearchWarehouseId) || parameters.SearchWarehouseId == Guid.Empty.ToString()))
                query = query.Where(e => e.Склад_Id == parameters.SearchWarehouseId);

            if (!(string.IsNullOrWhiteSpace(parameters.SearchDestinationId) || parameters.SearchDestinationId == "0"))
            {
                if (parameters.SearchDestinationId == Guid.Empty.ToString())
                    query = query.Where(e => e.НаправлениеДоставки_Id == Guid.Empty.ToString() && e.НаправлениеДоставкиРодитель_Id == Guid.Empty.ToString());
                else
                    query = query.Where(e => e.НаправлениеДоставки_Id == parameters.SearchDestinationId || e.НаправлениеДоставкиРодитель_Id == parameters.SearchDestinationId);
            }

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
                    case WhsOrderStatus.In.ToReceive:
                        query = query.Where(e => e.Статус == WhsOrderStatus.In.ToReceive)
                            .OrderByDescending(e => e.ВесовойКоэффициент).ThenBy(e => e.СрокВыполнения);
                        break;
                    case WhsOrderStatus.In.AtWork:
                        query = query.Where(e => e.Статус == WhsOrderStatus.In.AtWork)
                            .OrderByDescending(e => e.ВесовойКоэффициент).ThenBy(e => e.СрокВыполнения);
                        break;
                    case WhsOrderStatus.In.Received:
                        query = query.Where(e => e.Статус == WhsOrderStatus.In.Received)
                            .OrderBy(e => e.Data.Where(e => e.Статус == WhsOrderStatus.In.Received).OrderByDescending(d => d.DateTime).FirstOrDefault().DateTime);
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
