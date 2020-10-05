using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Whs.Shared.Models
{
    public static class WhsOrderExtensions
    {
        public static IQueryable<WhsOrderOut> Search(this IQueryable<WhsOrderOut> items, WhsOrderParameters parameters)
        {
            if (!(string.IsNullOrWhiteSpace(parameters.SearchWhsId) || parameters.SearchWhsId == Guid.Empty.ToString()))
                items = items.Where(e => e.Склад_Id == parameters.SearchWhsId);

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
