using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Whs.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToBitrixErrorsController : ControllerBase
    {
        private readonly string auth = "6c79855f0000071b0012a8e600000801000003a0ebaaed56832207fc9739fddb43d7ac";
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
