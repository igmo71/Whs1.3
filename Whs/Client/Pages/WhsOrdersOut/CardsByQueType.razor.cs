﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Whs.Client.Components;
using Whs.Shared.Models;

namespace Whs.Client.Pages.WhsOrdersOut
{
    public partial class CardsByQueType
    {
        [Parameter]
        public string NewTab { get; set; }
        [Inject]
        public HttpClient HttpClient { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }   
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private string Barcode;
        private Notification Notification;
        private WhsOrdersDtoOut OrdersDto;
        private WhsOrderParameters OrderParameters;
        private Warehouse[] Warehouses;
        private Destination[] Destinations;
        private SearchByNumber SearchByNumber;
        private SearchByDestination SearchByDestination;


        protected override async Task OnInitializedAsync()
        {
            OrderParameters = new WhsOrderParameters();
            await GetWarehousesAsync();
            await GetDestinationsAsync();
            await GetOrdersDtoAsync();
        }

        private async Task GetWarehousesAsync()
        {
            Warehouses = await HttpClient.GetFromJsonAsync<Warehouse[]>("api/Warehouses/ForOut");
        }

        private async Task GetDestinationsAsync()
        {
            Destinations = await HttpClient.GetFromJsonAsync<Destination[]>("api/Destinations");
        }

        private async Task GetOrdersDtoAsync()
        {
            DateTime beginTime = DateTime.Now;
            System.Console.WriteLine("GetOrdersDtoAsync - begin");
            string requestUri = $"api/WhsOrdersOut/DtoByQueType?" +
                $"SearchBarcode={OrderParameters.SearchBarcode}&" +
                $"SearchTerm={OrderParameters.SearchTerm}&" +
                $"SearchWhsId={OrderParameters.SearchWhsId}&" +
                $"SearchDestinationId={OrderParameters.SearchDestinationId}";
            try
            {
                OrdersDto = await HttpClient.GetFromJsonAsync<WhsOrdersDtoOut>(requestUri);
            }
            catch
            {
                await Notification.ShowAsync($"Не найдено", 1);
            }
            //StateHasChanged();
            System.Console.WriteLine($"GetOrdersDtoAsync - duration: {DateTime.Now - beginTime}");
        }
        private async Task SearchByWarehouseAsync(string searchStorageId)
        {
            OrderParameters.SearchBarcode = null;
            OrderParameters.SearchWhsId = searchStorageId;
            await GetOrdersDtoAsync();
        }

        private async Task SearchByNumberAsync(string searchTerm)
        {
            OrderParameters.SearchBarcode = null;
            OrderParameters.SearchTerm = searchTerm;
            await GetOrdersDtoAsync();
        }

        private async Task SearchByDestinationsAsync(string searchDestinationId)
        {
            OrderParameters.SearchBarcode = null;
            OrderParameters.SearchDestinationId = searchDestinationId;
            await GetOrdersDtoAsync();
        }

        private void SearchClear()
        {
            SearchByDestination.Clear();
            SearchByNumber.SearchTerm = string.Empty;
            OrderParameters.SearchTerm = null;
            OrderParameters.SearchDestinationId = null;
        }
        private async Task ScannedBarcodeAsync(ChangeEventArgs args)
        {
            Barcode = args.Value.ToString();
            await SearchByBarcodeAsync();
        }

        private async Task SearchByBarcodeAsync()
        {
            SearchClear();
            OrderParameters.SearchBarcode = Barcode;
            await GetOrdersDtoAsync();

            if (!string.IsNullOrEmpty(OrdersDto.SingleId))
            {
                await OpenItemAsync(OrdersDto.SingleId);
            }
        }

        private async Task SearchByBarcodeClearAsync()
        {
            OrdersDto.SingleId = null;
            OrdersDto.MngrOrderName = null;
            OrderParameters.SearchBarcode = null;
            await GetOrdersDtoAsync();
        }
        private async Task OpenItemAsync(string id)
        {
            if (NewTab == "NewTab")
            {
                OrderParameters.SearchBarcode = null;
                await JSRuntime.InvokeVoidAsync("window.open", $"/WhsOutItem/{id}", "_blank");
            }
            else
                NavigationManager.NavigateTo($"/WhsOutItem/{id}");
        }

        private async Task PrintAsync() => await JSRuntime.InvokeVoidAsync("print");
    }
}
