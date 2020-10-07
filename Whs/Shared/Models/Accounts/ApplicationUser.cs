using Microsoft.AspNetCore.Identity;

namespace Whs.Shared.Models.Accounts
{
    public class ApplicationUser : IdentityUser
    {
        public string Barcode { get; set; }
        public string FullName { get; set; }

        public Warehouse Warehouse { get; set; }
        public string WarehouseId { get; set; }
    }
}
