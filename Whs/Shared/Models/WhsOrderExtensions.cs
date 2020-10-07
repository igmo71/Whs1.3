using System;
using System.Linq;

namespace Whs.Shared.Models
{
    public static class WhsOrderExtensions
    {
        public static IQueryable<WhsOrderOut> Search(this IQueryable<WhsOrderOut> items, WhsOrderParameters parameters)
        {
            if (!(string.IsNullOrWhiteSpace(parameters.SearchWarehouseId) || parameters.SearchWarehouseId == Guid.Empty.ToString()))
                items = items.Where(e => e.Склад_Id == parameters.SearchWarehouseId);

            if (!(string.IsNullOrWhiteSpace(parameters.SearchDestinationId) || parameters.SearchDestinationId == "0"))
                items = items.Where(e => e.НаправлениеДоставки_Id == parameters.SearchDestinationId);

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var lowerCaseSearchTerm = parameters.SearchTerm.Trim().ToLower();
                items = items.Where(e => e.Номер.ToLower().Contains(lowerCaseSearchTerm));
            }
            return items;
        }
    }
}
