using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using Whs.Server.Data;
using Whs.Shared.Models;
using Whs.Shared.Models.Accounts;
using Whs.Shared.Utils;

namespace Whs.Server.Pages.Accounts
{
    public class ListModel : PageModel
    {
        //private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly QRCodeGenerator _qrGenerator;

        public ListModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _qrGenerator = new QRCodeGenerator();
        }

        public ApplicationUser[] ApplicationUsers { get; set; }

        public void OnGet()
        {
            ApplicationUsers = _userManager.Users.OrderBy(e => e.WarehouseId).ThenBy(e => e.FullName).AsNoTracking().ToArray();
        }

        public IActionResult OnGetLoadQr(string name)

        {
            string id = name.Replace(".png", "");
            byte[] buf = GetQrBytes(GuidConvert.ToNumStr(id));
            return File(buf, "image/png", id);
        }

        private byte[] GetQrBytes(string text)
        {
            text ??= "";
            QRCodeData qrCodeData = _qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            using MemoryStream stream = new MemoryStream();
            qrCodeImage.Save(stream, ImageFormat.Png);
            return stream.ToArray();
        }
    }
}
