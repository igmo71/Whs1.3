using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Whs.Shared.Models;

namespace Whs.Client.Pages.WhsOrdersOut
{
    public partial class Item
    {
        [Parameter]
        public string Id { get; set; }
        [Inject]
        HttpClient HttpClient { get; set; }

        WhsOrderDtoOut OrderDto;


        protected override async Task OnInitializedAsync()
        {
            try
            {
                 OrderDto = await HttpClient.GetFromJsonAsync<WhsOrderDtoOut>($"api/WhsOrdersOut/Dto/{Id}");
            }
            catch
            {

            }
        }
    }
}
