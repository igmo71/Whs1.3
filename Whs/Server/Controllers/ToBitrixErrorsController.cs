using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Whs.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToBitrixErrorsController : ControllerBase
    {
        private readonly string[] to = { "318", "2844" };
        // api/ToBitrixErrors/message
        [HttpPost("{message}")]
        public async Task PostAsync(string message = "Hello, World!")
        {
            HttpClient _clientBitrix = new HttpClient { BaseAddress = new Uri("https://portal.dobroga.ru/rest/2049/yyvnhvfv64px921o/im.notify.json") };
            string requestUri;
            HttpResponseMessage response;
            foreach (var item in to)
            {
                requestUri = $"?message={message}&to={item}";
                response = await _clientBitrix.PostAsync(requestUri, null);
            }
        }
    }
}
