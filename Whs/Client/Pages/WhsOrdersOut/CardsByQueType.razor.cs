using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Whs.Client.Components;
using Whs.Shared.Models;

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

        private WhsOrdersDtoOut WhsOrdersDtoOut;

        private WhsOrderParameters OrderParameters;
        private SearchByNumber SearchByNumber;
        private SearchByDestination SearchByDestination;
        private Warehouse[] Warehouses;
        private Destination[] Destinations;


        protected override async Task OnInitializedAsync()
        {
            DateTime timeBegin = DateTime.Now;
            OrderParameters = new WhsOrderParameters();
            await GetWarehousesAsync();
            await GetDestinationsAsync();
            await GetWhsOrdersDtoOutAsync();
            Console.WriteLine($"OnInitializedAsync: {DateTime.Now - timeBegin}");
        }

        private async Task GetWarehousesAsync()
        {
            Warehouses = await HttpClient.GetFromJsonAsync<Warehouse[]>("api/Warehouses/ForOut");
        }

        private async Task GetDestinationsAsync()
        {
            Destinations = await HttpClient.GetFromJsonAsync<Destination[]>("api/Destinations");
        }

        private async Task GetWhsOrdersDtoOutAsync()
        {
            string requestUri = $"api/WhsOrdersOut/DtoByQueType?" +
                $"SearchBarcode={OrderParameters.SearchBarcode}&" +
                $"SearchTerm={OrderParameters.SearchTerm}&" +
                $"SearchWhsId={OrderParameters.SearchWhsId}&" +
                $"SearchDestinationId={OrderParameters.SearchDestinationId}";

            try
            {
                WhsOrdersDtoOut = await HttpClient.GetFromJsonAsync<WhsOrdersDtoOut>(requestUri);
            }
            catch
            {
                await Notification.ShowAsync($"Не найдено", 1);
            }
            StateHasChanged();
        }

    }
}
