using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Timers;
using Whs.Client.Components;
using Whs.Shared.Models;

namespace Whs.Client.Pages.WhsOrdersOut
{
    public partial class CardsByQueType : IDisposable
    {
        [Parameter]
        public string SearchStatus { get; set; }
        [Inject]
        public HttpClient HttpClient { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IConfiguration Configuration { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }

        private Timer Timer;
        private string Barcode;
        private Notification Notification;
        private WhsOrdersDtoOut OrdersDto;
        private WhsOrderParameters OrderParameters;
        private Warehouse[] Warehouses;
        private Destination[] Destinations;
        private SearchByNumber SearchByNumber;
        private SearchByDestination SearchByDestination;
        private Dictionary<string, string> SearchStatusButtons;
        private string warehouseId;

        protected override async Task OnInitializedAsync()
        {
            DateTime beginTime = DateTime.Now;
            Console.WriteLine("OnInitializedAsync - begin");
            OrderParameters = new WhsOrderParameters();
            CreateSearchStatusButtons();
            await GetWarehouseIdAsync();
            await GetWarehousesAsync();
            await GetDestinationsAsync();
            await GetOrdersDtoAsync();
            SetTimer(double.Parse(Configuration["TimerInterval"]), true);
            Console.WriteLine($"OnInitializedAsync - duration: {DateTime.Now - beginTime}");
        }

        private void CreateSearchStatusButtons(string initStatus = "Подготовлено")
        {
            SearchStatusButtons = new Dictionary<string, string>
                { { "Подготовлено", "" }, { "К отбору", ""}, { "К отгрузке", ""}, { "Отгружен", ""} };
            OrderParameters.SearchStatus = string.IsNullOrEmpty(SearchStatus) ? initStatus : SearchStatus;
            if (string.IsNullOrEmpty(SearchStatus))
                SearchStatusButtons[initStatus] = "active";
            else
                SearchStatusButtons[SearchStatus] = "active";
        }

        private async Task GetWarehouseIdAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity.IsAuthenticated)
            {
                warehouseId = user.FindFirst(c => c.Type == ClaimTypes.GroupSid)?.Value;
                if (!user.IsInRole("Manager"))
                    OrderParameters.SearchWarehouseId = warehouseId;
            }
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
            try
            {
                DateTime beginTime = DateTime.Now;
                Console.WriteLine("GetOrdersDtoAsync - begin");
                string requestUri = $"api/WhsOrdersOut/DtoByQueType?" +
                    $"SearchBarcode={OrderParameters.SearchBarcode}&" +
                    $"SearchStatus={OrderParameters.SearchStatus}&" +
                    $"SearchTerm={OrderParameters.SearchTerm}&" +
                    $"SearchWarehouseId={OrderParameters.SearchWarehouseId}&" +
                    $"SearchDestinationId={OrderParameters.SearchDestinationId}";
                Console.WriteLine($"GetOrdersDtoAsync - requestUri: {requestUri}");
                OrdersDto = await HttpClient.GetFromJsonAsync<WhsOrdersDtoOut>(requestUri);
                StateHasChanged();
                if (OrdersDto.Items.Count == 0)
                {
                    await Notification.ShowAsync($"Не найдено", 1);
                    if (OrderParameters.SearchBarcode != null)
                        await SearchByBarcodeClearAsync();
                }
                Console.WriteLine($"GetOrdersDtoAsync - duration: {DateTime.Now - beginTime}");
            }
            catch (Exception ex)
            {
                await Notification.ShowAsync($"Ошибка загрузки ордеров. {Environment.NewLine}{ex.Message}", 2);
                Console.WriteLine($"GetOrdersDtoAsync - Exception: {ex.Message}");
                Console.WriteLine($"{ex.StackTrace}");
            }
        }

        private async Task SearchByWarehouseAsync(string searchWarehouseId)
        {
            OrderParameters.SearchBarcode = null;
            OrderParameters.SearchWarehouseId = searchWarehouseId;
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

        private async Task SearchByStatus(string searchStatus)
        {
            OrderParameters.SearchBarcode = null;
            OrderParameters.SearchStatus = searchStatus;
            await GetOrdersDtoAsync();

            var enumerator = SearchStatusButtons.GetEnumerator();

            foreach (var key in SearchStatusButtons.Keys.ToArray())
            {
                SearchStatusButtons[key] = string.Empty;
            }
            SearchStatusButtons[searchStatus] = "active";
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
                OpenItem(OrdersDto.SingleId);
            }
        }

        private async Task SearchByBarcodeClearAsync()
        {
            OrdersDto.SingleId = null;
            OrdersDto.MngrOrderName = null;
            OrderParameters.SearchBarcode = null;
            await GetOrdersDtoAsync();
        }

        private void OpenItem(string id)
        {
            NavigationManager.NavigateTo($"WhsOrdersOut/Item/{id}/{OrderParameters.SearchStatus}");
        }

        public void SetTimer(double interval, bool autoReset)
        {
            Timer = new Timer(interval * 1000);
            Timer.Elapsed += async delegate
            {
                Console.WriteLine($"Timer.Elapsed at: {DateTime.Now}");
                await GetOrdersDtoAsync();
                await GetDestinationsAsync();
            };
            Timer.AutoReset = autoReset;
            Timer.Enabled = true;
        }

        public void Dispose()
        {
            if (Timer != null)
                Timer.Dispose();
        }

        private void Print()
        {
            string searchParameters =
                       $"SearchBarcode={OrderParameters.SearchBarcode}&" +
                       $"SearchStatus={OrderParameters.SearchStatus}&" +
                       $"SearchTerm={OrderParameters.SearchTerm}&" +
                       $"SearchWarehouseId={OrderParameters.SearchWarehouseId}&" +
                       $"SearchDestinationId={OrderParameters.SearchDestinationId}";
            NavigationManager.NavigateTo($"WhsOrdersOut/PrintList/{searchParameters}/{OrderParameters.SearchStatus}");
        }
    }
}
