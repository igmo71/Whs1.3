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
        [Parameter] public string SearchStatus { get; set; }
        [Parameter] public string SearchDestinationId { get; set; }
        [Inject] public HttpClient HttpClient { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IConfiguration Configuration { get; set; }
        [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; }

        private Timer Timer;
        private string Barcode;
        private WhsOrdersDtoOut OrdersDto;
        private WhsOrderParameters OrderParameters;
        private string SearchParameters;
        private Warehouse[] Warehouses;
        private Dictionary<string, string> SearchStatusButtons;
        private string warehouseId;

        protected override async Task OnInitializedAsync()
        {
            await SetOrderParametersAsync();
            await GetWarehousesAsync();
            await GetOrdersDtoAsync();
            SetTimer(double.Parse(Configuration["TimerInterval"]), true);
            SearchByDestination.Set(SearchDestinationId);
        }

        private async Task SetOrderParametersAsync()
        {
            OrderParameters = new WhsOrderParameters
            {
                SearchStatus = string.IsNullOrEmpty(SearchStatus) ? WhsOrderStatus.Out.Prepared : SearchStatus,
                SearchDestinationId = string.IsNullOrEmpty(SearchDestinationId) ? "" : SearchDestinationId
            };

            AuthenticationState authState = await AuthStateProvider.GetAuthenticationStateAsync();
            ClaimsPrincipal user = authState.User;
            if (user.Identity.IsAuthenticated)
            {
                warehouseId = user.FindFirst(c => c.Type == ClaimTypes.GroupSid)?.Value;
                if (!user.IsInRole("Manager"))
                    OrderParameters.SearchWarehouseId = warehouseId;
            }

            SearchStatusButtons = new Dictionary<string, string>
            {
                { WhsOrderStatus.Out.Prepared, "" },
                { WhsOrderStatus.Out.ToCollect, ""},
                { WhsOrderStatus.Out.ToShipment, ""},
                { WhsOrderStatus.Out.Shipped, ""}
            };
            SearchStatusButtons[OrderParameters.SearchStatus] = "active";
        }

        private async Task GetWarehousesAsync()
        {
            try
            {
                Warehouses = await HttpClient.GetFromJsonAsync<Warehouse[]>("api/Warehouses/ForOut");
            }
            catch (Exception ex)
            {
                await Notification.ShowAsync($"Ошибка загрузки cписка складов.", 1);
                await ToBitrixErrors($"Ошибка загрузки cписка складов - {ex.Message}");
            }
        }

        private async Task GetOrdersDtoAsync()
        {
            try
            {
                SearchParameters =
                    $"SearchBarcode={OrderParameters.SearchBarcode}&" +
                    $"SearchStatus={OrderParameters.SearchStatus}&" +
                    $"SearchTerm={OrderParameters.SearchTerm}&" +
                    $"SearchWarehouseId={OrderParameters.SearchWarehouseId}&" +
                    $"SearchDestinationId={OrderParameters.SearchDestinationId}";
                OrdersDto = await HttpClient.GetFromJsonAsync<WhsOrdersDtoOut>($"api/WhsOrdersOut/DtoByQueType?{SearchParameters}");
                StateHasChanged();
                if (OrdersDto.Items.Count == 0)
                {
                    if (OrderParameters.SearchBarcode != null)
                    {
                        await Notification.ShowAsync($"По штрих коду ничего не найдено.", 1);
                        await SearchByBarcodeClearAsync(isGetOrders: true);
                    }
                }
            }
            catch (Exception ex)
            {
                await Notification.ShowAsync($"Ошибка загрузки расходных ордеров.", 2);
                await ToBitrixErrors($"Ошибка загрузки расходных ордеров - {ex.Message}");
            }
        }

        private async Task SearchByWarehouseAsync(string searchWarehouseId)
        {
            await SearchByBarcodeClearAsync();
            OrderParameters.SearchWarehouseId = searchWarehouseId;
            await GetOrdersDtoAsync();
        }

        private async Task SearchByNumberAsync(string searchTerm)
        {
            await SearchByBarcodeClearAsync();
            OrderParameters.SearchTerm = searchTerm;
            await GetOrdersDtoAsync();
        }

        private void SearchByNumberClear()
        {
            SearchByNumber.SearchTerm = "";
            OrderParameters.SearchTerm = null;
        }

        private async Task SearchByDestinationsAsync(string searchDestinationId)
        {
            await SearchByBarcodeClearAsync();
            OrderParameters.SearchDestinationId = searchDestinationId;
            await GetOrdersDtoAsync();
        }

        private void SearchByDestinationsClear()
        {
            SearchDestinationId = "";
            SearchByDestination.Set();
            OrderParameters.SearchDestinationId = null;
        }

        private async Task SearchByStatus(string searchStatus)
        {
            await SearchByBarcodeClearAsync();
            SearchByDestinationsClear();
            OrderParameters.SearchStatus = searchStatus;
            await GetOrdersDtoAsync();

            foreach (var key in SearchStatusButtons.Keys.ToArray())
                SearchStatusButtons[key] = "";
            SearchStatusButtons[searchStatus] = "active";
        }

        private async Task ScannedBarcodeAsync(ChangeEventArgs args)
        {
            Barcode = args.Value.ToString();
            await SearchByBarcodeAsync();
        }

        private async Task SearchByBarcodeAsync()
        {
            SearchByNumberClear();
            SearchByDestinationsClear();
            OrderParameters.SearchBarcode = Barcode;
            await GetOrdersDtoAsync();
            if (!string.IsNullOrEmpty(OrdersDto.SingleId))
            {
                OpenItem(OrdersDto.SingleId);
            }
        }

        private async Task SearchByBarcodeClearAsync(bool isGetOrders = false)
        {
            OrdersDto.SingleId = null;
            OrdersDto.MngrOrderName = null;
            OrderParameters.SearchBarcode = null;
            if (isGetOrders)
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
                await GetOrdersDtoAsync();
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
            NavigationManager.NavigateTo($"WhsOrdersOut/PrintList/{OrderParameters.SearchWarehouseId}/{OrderParameters.SearchStatus}/{OrderParameters.SearchDestinationId}");
        }

        private async Task ToBitrixErrors(string message)
        {
            await HttpClient.PostAsync($"api/ToBitrixErrors/{message}", null);
        }
    }
}
