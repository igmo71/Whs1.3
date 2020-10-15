using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Timers;
using Whs.Shared.Models;

namespace Whs.Client.Pages.WhsOrdersOut
{
    public partial class ShippingWorkplace
    {
        [Inject] HttpClient HttpClient { get; set; }
        [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; }
        
        private Timer Timer;
        private string Barcode;
        private WhsOrderOut[] Orders;
        private string warehouseId; 

        protected override async Task OnInitializedAsync()
        {
            await GetWarehouseIdAsync();
            await GetOrdersAsync();
        }

        private async Task GetWarehouseIdAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity.IsAuthenticated)
            {
                warehouseId = user.FindFirst(c => c.Type == ClaimTypes.GroupSid)?.Value;
            }
        }

        private async Task GetOrdersAsync()
        {
            Orders = await HttpClient.GetFromJsonAsync<WhsOrderOut[]>($"api/WhsOrdersOut/Shipping/{warehouseId}");
        }

        private async Task ScannedBarcodeAsync(ChangeEventArgs args)
        {
            try
            {
                Notification.Show($"Запрос отгрузки...");
                Barcode = args.Value.ToString();
                HttpResponseMessage response = await HttpClient.PutAsJsonAsync<WhsOrderOut>($"api/WhsOrdersOut/Shipping/{Barcode}", null);
                string content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    await GetOrdersAsync();
                    await Notification.HideAsync($"{content} - Отгружен", 1);
                }
                else
                {
                    await Notification.HideAsync("Отгрузить не удалось.", 1);
                }
            }
            catch (Exception ex)
            {
                await Notification.ShowAsync($"Ошибка отгрузки.", 1);
                Console.WriteLine($"ScannedBarcodeAsync - Exception: {ex.Message}");
                Console.WriteLine($"{ex.StackTrace}");
            }
        }
    }
}
