using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Whs.Client.Components;

namespace Whs.Client.Pages.WhsOrdersOut
{
    public partial class CardsByQueType
    {
        [Inject]
        public HttpClient HttpClient { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private Notification Notification;
        private string Barcode;
    }
}
