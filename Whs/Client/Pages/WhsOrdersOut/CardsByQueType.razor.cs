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
        [Parameter]
        public string NewTab { get; set; }
        [Inject]
        public HttpClient HttpClient { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private Notification Notification;
        private string Barcode;

        private WhsOrdersDtoOut WhsOrdersDto;

        private WhsOrderParameters WhsOrderParameters;
        private SearchByNumber SearchByNumber;
        private SearchByDestination SearchByDestination;
        private Warehouse[] Warehouses;
        private Destination[] Destinations;


        protected override async Task OnInitializedAsync()
        {
            WhsOrderParameters = new WhsOrderParameters();
            await GetWarehousesAsync();
            await GetDestinationsAsync();
            await GetWhsOrdersDtoAsync();
        }

        private async Task GetWarehousesAsync()
        {
            Warehouses = await HttpClient.GetFromJsonAsync<Warehouse[]>("api/Warehouses/ForOut");
        }

        private async Task GetDestinationsAsync()
        {
            Destinations = await HttpClient.GetFromJsonAsync<Destination[]>("api/Destinations");
        }

        private async Task GetWhsOrdersDtoAsync()
        {
            string requestUri = $"api/WhsOrdersOut/DtoByQueType?" +
                $"SearchBarcode={WhsOrderParameters.SearchBarcode}&" +
                $"SearchTerm={WhsOrderParameters.SearchTerm}&" +
                $"SearchWhsId={WhsOrderParameters.SearchWhsId}&" +
                $"SearchDestinationId={WhsOrderParameters.SearchDestinationId}";

            try
            {
                WhsOrdersDto = await HttpClient.GetFromJsonAsync<WhsOrdersDtoOut>(requestUri);
            }
            catch
            {
                await Notification.ShowAsync($"Не найдено", 1);
            }
            //StateHasChanged();
        }
        private async Task SearchByNumberAsync(string searchTerm)
        {
            WhsOrderParameters.SearchBarcode = null;
            WhsOrderParameters.SearchTerm = searchTerm;
            await GetWhsOrdersDtoAsync();
        }

        private async Task SearchByWarehouseAsync(string searchStorageId)
        {
            WhsOrderParameters.SearchBarcode = null;
            WhsOrderParameters.SearchWhsId = searchStorageId;
            await GetWhsOrdersDtoAsync();
        }

        private async Task SearchByDestinationsAsync(string searchDestinationId)
        {
            WhsOrderParameters.SearchBarcode = null;
            WhsOrderParameters.SearchDestinationId = searchDestinationId;
            await GetWhsOrdersDtoAsync();
        }

        private void SearchClear()
        {
            //SearchByNumber.Clear();
            SearchByNumber.SearchTerm = string.Empty;
            SearchByDestination.Clear();
            WhsOrderParameters.SearchTerm = null;
            WhsOrderParameters.SearchDestinationId = null;
        }
        private async Task ScannedBarcodeAsync(ChangeEventArgs args)
        {
            Barcode = args.Value.ToString();
            await SearchByBarcodeAsync();
        }

        private async Task SearchByBarcodeAsync()
        {
            SearchClear();
            WhsOrderParameters.SearchBarcode = Barcode;
            await GetWhsOrdersDtoAsync();

            if (!string.IsNullOrEmpty(WhsOrdersDto.SingleId))
            {
                await OpenItemAsync(WhsOrdersDto.SingleId);
            }
        }

        private async Task SearchByBarcodeClearAsync()
        {
            WhsOrdersDto.SingleId = null;
            WhsOrdersDto.MngrOrderName = null;
            WhsOrderParameters.SearchBarcode = null;
            await GetWhsOrdersDtoAsync();
        }
        private async Task OpenItemAsync(string id)
        {
            if (NewTab == "NewTab")
            {
                WhsOrderParameters.SearchBarcode = null;
                await JSRuntime.InvokeVoidAsync("window.open", $"/storageordersout/{id}", "_blank");
            }
            else
                NavigationManager.NavigateTo($"/storageordersout/{id}");
        }

        private async Task PrintAsync() => await JSRuntime.InvokeVoidAsync("print");
    }
}
