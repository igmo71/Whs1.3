using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Whs.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToBitrixErrorsController : ControllerBase
    {
        private readonly ILogger<ToBitrixErrorsController> _logger;
        private readonly HttpClient _bitrixClient;
        private readonly bool isSendError;
        private readonly string[] recipients;

        public ToBitrixErrorsController(IConfiguration configuration, IHttpClientFactory clientFactory, ILogger<ToBitrixErrorsController> logger)
        {
            _bitrixClient = clientFactory.CreateClient("ErrorBitrixClient");
            isSendError = configuration.GetSection("IsSendErrorToBitrix").Get<bool>();
            recipients = configuration.GetSection("BitrixErrorRecipients").Get<string[]>();
            _logger = logger;
        }

        // api/ToBitrixErrors/message
        [HttpPost("{message}")]
        public async Task PostAsync(string message)
        {
            _logger.LogError($"---> PostAsync: Client error; message={message}");
            if (isSendError)
            {
                try
                {
                    foreach (string recipient in recipients)
                    {
                        _ = await _bitrixClient.PostAsync($"?message={message}&to={recipient}", null);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"---> PostAsync: Exception; Message = {ex.Message}");
                }

            }
        }
    }
}
