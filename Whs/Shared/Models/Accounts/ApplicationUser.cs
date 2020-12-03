using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Whs.Shared.Utils;

namespace Whs.Shared.Models.Accounts
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; set; }
        [Column("WarehouseId")]
        public string WarehouseId { get; set; }

        [NotMapped]
        public string Barcode => GuidConvert.ToNumStr(Id);

        [NotMapped]
        public string Surname => string.IsNullOrEmpty(FullName) ? "" : FullName.Substring(0, FullName.IndexOf(' '));
    }
    public class ApplicationUserParameters
    {
        public string SearchTerm { get; set; }
        public string SearchWarehouseId { get; set; }
    }

    public static class ApplicationUserExtensions
    {

        public static IQueryable<ApplicationUser> Search(this IQueryable<ApplicationUser> users, ApplicationUserParameters parameters)
        {
            if (!(string.IsNullOrWhiteSpace(parameters.SearchWarehouseId) || parameters.SearchWarehouseId == Guid.Empty.ToString()))
                users = users.Where(e => e.WarehouseId == parameters.SearchWarehouseId);

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var lowerCaseSearchTerm = parameters.SearchTerm.Trim().ToLower();
                users = users.Where(e => e.FullName.ToLower().Contains(lowerCaseSearchTerm));
            }

            return users;
        }
    }
}
