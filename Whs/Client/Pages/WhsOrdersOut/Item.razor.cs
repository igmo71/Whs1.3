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

        WhsOrderDtoOut WhsOrderDto;


        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine("OnInitializedAsync - start");

            //var response = await HttpClient.GetAsync($"api/WhsOrdersOut/Dto/{Id}");
            //var content = await response.Content.ReadAsStringAsync();
            //WhsOrderDto = System.Text.Json.JsonSerializer.Deserialize<WhsOrderDtoOut>(content);

            WhsOrderDto = await HttpClient.GetFromJsonAsync<WhsOrderDtoOut>($"api/WhsOrdersOut/Dto/{Id}");


            Console.WriteLine("OnInitializedAsync - end");
        }
    }
}
