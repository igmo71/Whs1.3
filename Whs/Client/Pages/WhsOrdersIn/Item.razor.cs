using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Whs.Client.Components;
using Whs.Shared.Models;

namespace Whs.Client.Pages.WhsOrdersIn
{
    public partial class Item
    {
        [Parameter] public string Id { get; set; }
        [Parameter] public string SearchStatus { get; set; }
        [Inject] HttpClient HttpClient { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        private string Barcode;
        WhsOrderDtoIn OrderDto;
        public EditingCause[] EditingCauses { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetEditingCausesAsync();
            await GetOrderDtoAsync();
        }

        private async Task GetEditingCausesAsync()
        {
            EditingCauses = await HttpClient.GetFromJsonAsync<EditingCause[]>("api/EditingCauses/ForIn");
        }

        private async Task GetOrderDtoAsync()
        {
            try
            {
                DateTime beginTime = DateTime.Now;
                OrderDto = await HttpClient.GetFromJsonAsync<WhsOrderDtoIn>($"api/WhsOrdersIn/Dto/{Id}", new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                StateHasChanged();
                Console.WriteLine($"GetOrderDtoAsync - duration: {DateTime.Now - beginTime}");
            }
            catch (Exception ex)
            {
                await Notification.ShowAsync($"Ошибка загрузки ордера.", 1);
                Console.WriteLine($"GetOrderDtoAsync - Excepton: {ex.Message}");
                Console.WriteLine($"{ex.StackTrace}");
                await ToBitrixErrors($"Ошибка загрузки ордера: {Id}: {ex.Message}");
            }
        }

        private async Task ScannedBarcodeAsync(ChangeEventArgs args)
        {
            try
            {
                DateTime beginTime = DateTime.Now;
                Notification.Show($"Запрос изменения статуса...");
                Barcode = args.Value.ToString();
                HttpResponseMessage response = await HttpClient.PutAsJsonAsync<WhsOrderIn>($"api/WhsOrdersIn/{OrderDto.Item.Документ_Id}/{Barcode}", OrderDto.Item);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    await GetOrderDtoAsync();
                    await Notification.HideAsync($"{OrderDto.Item.Документ_Name} - {OrderDto.Item.Статус}", 1);
                    await ToBitrixErrors($"{OrderDto.Item.Документ_Name} - {OrderDto.Item.Статус}");
                    if (OrderDto.Item.Статус == WhsOrderStatus.In.AtWork)
                    {
                        await PrintAsync();
                    }
                    Return();
                }
                else
                {
                    await Notification.HideAsync("Cтатус изменить не удалось", 1);
                    await ToBitrixErrors($"Cтатус изменить не удалось: {OrderDto.Item.Документ_Name}");
                }
                Console.WriteLine($"ScannedBarcodeAsync - duration: {DateTime.Now - beginTime}");
            }
            catch (Exception ex)
            {
                await Notification.ShowAsync($"Ошибка изменения статуса.", 1);
                Console.WriteLine($"ScannedBarcodeAsync - Exception: {ex.Message}");
                Console.WriteLine($"{ex.StackTrace}");
                await ToBitrixErrors($"Ошибка изменения статуса: {OrderDto.Item.Документ_Name} - {ex.Message}");
            }
        }

        private async Task PrintAsync() => await JSRuntime.InvokeVoidAsync("print");

        private void Return()
        {
            NavigationManager.NavigateTo($"WhsOrdersIn/CardsByQueType/{SearchStatus}");
        }

        private async Task ToBitrixErrors(string message)
        {
            await HttpClient.PostAsync($"api/ToBitrixErrors/{message}", null);
        }
    }
}
