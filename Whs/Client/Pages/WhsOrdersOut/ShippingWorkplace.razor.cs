using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Whs.Shared.Models;

namespace Whs.Client.Pages.WhsOrdersOut
{
    public partial class ShippingWorkplace
    {
        [Inject] HttpClient HttpClient { get; set; }
        [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; }
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
            Orders = await HttpClient.GetFromJsonAsync<WhsOrderOut[]>($"api/WhsOrdersOut/ForShipment/{warehouseId}");
        }
    }
}
