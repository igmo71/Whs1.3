using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Whs.Shared.Models;

namespace Whs.Client.Pages.WhsOrdersIn
{
    public partial class PrintList
    {
        [Parameter] public string SearchWarehouseId { get; set; }
        [Parameter] public string SearchStatus { get; set; }
        [Inject] public HttpClient HttpClient { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        private WhsOrderIn[] Orders;

        protected override async Task OnInitializedAsync()
        {
            await GetOrdersDtoAsync();
        }

        private async Task GetOrdersDtoAsync()
        {
            try
            {
                Orders = await HttpClient.GetFromJsonAsync<WhsOrderIn[]>($"api/WhsOrdersIn/PrintList?SearchWarehouseId={SearchWarehouseId}&SearchStatus={SearchStatus}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetOrdersDtoAsync - Exception: {ex.Message}");
            }
        }

        private async Task PrintAsync()
        {
            await JSRuntime.InvokeVoidAsync("print");
            Return();
        }

        private void Return()
        {
            NavigationManager.NavigateTo($"WhsOrdersIn/CardsByQueType/{SearchStatus}");
        }
    }
}
