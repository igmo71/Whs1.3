using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Timers;
using Whs.Shared.Models;

namespace Whs.Client.Pages.WhsOrdersOut
{
    public partial class Shipment : IDisposable
    {
        [Inject] HttpClient HttpClient { get; set; }
        [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject] public IConfiguration Configuration { get; set; }

        private Timer Timer;
        private string Barcode;
        private WhsOrderOut[] Orders;
        private string warehouseId;

        protected override async Task OnInitializedAsync()
        {
            await GetWarehouseIdAsync();
            await GetOrdersAsync();
            SetTimer(double.Parse(Configuration["TimerInterval"]), true);
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
            Orders = await HttpClient.GetFromJsonAsync<WhsOrderOut[]>($"api/WhsOrdersOut/Shipment/{warehouseId}");
            StateHasChanged();
        }

        private async Task ScannedBarcodeAsync(ChangeEventArgs args)
        {
            try
            {
                Notification.Show($"Запрос отгрузки...");
                Barcode = args.Value.ToString();
                HttpResponseMessage response = await HttpClient.PutAsJsonAsync<WhsOrderOut>($"api/WhsOrdersOut/Shipment/{Barcode}", null);
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
                Console.WriteLine($"ScannedBarcodeAsync Exception: {ex.Message}");
            }
        }

        public void SetTimer(double interval, bool autoReset)
        {
            Timer = new Timer(interval * 1000);
            Timer.Elapsed += async delegate
            {
                await GetOrdersAsync();
            };
            Timer.AutoReset = autoReset;
            Timer.Enabled = true;
        }

        public void Dispose()
        {
            if (Timer != null)
                Timer.Dispose();
        }
    }
}
