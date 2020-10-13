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
        private Notification Notification;
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
                Console.WriteLine("GetOrderDtoAsync - begin");
                OrderDto = await HttpClient.GetFromJsonAsync<WhsOrderDtoIn>($"api/WhsOrdersIn/Dto/{Id}", new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                Console.WriteLine($"GetOrderDtoAsync - OrderDto.Item.Статус: {OrderDto.Item.Статус}");
                StateHasChanged();
                Console.WriteLine($"GetOrderDtoAsync - duration: {DateTime.Now - beginTime}");
            }
            catch (Exception ex)
            {
                await Notification.ShowAsync($"Ошибка загрузки ордера. {Environment.NewLine}{ex.Message}", 2);
                Console.WriteLine($"GetOrderDtoAsync - Excepton: {ex.Message}");
                Console.WriteLine($"{ex.StackTrace}");
            }
        }

        private async Task ScannedBarcodeAsync(ChangeEventArgs args)
        {
            try
            {
                DateTime beginTime = DateTime.Now;
                Console.WriteLine("ScannedBarcodeAsync - begin");
                Notification.Show($"Запрос на изменение статуса...");
                Barcode = args.Value.ToString();
                HttpResponseMessage response = await HttpClient.PutAsJsonAsync<WhsOrderIn>($"api/WhsOrdersIn/{OrderDto.Item.Документ_Id}/{Barcode}", OrderDto.Item);
                Console.WriteLine($"HttpResponseMessage - response: {response.StatusCode} - {response.ReasonPhrase} - {await response.Content.ReadAsStringAsync()}");
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    if (OrderDto.Item.Статус == "Подготовлено")
                    {
                        await GetOrderDtoAsync();
                        await PrintAsync();
                    }
                    await Notification.HideAsync("Ok", 1);
                    await ToBitrixErrors($"Cтатус изменен: {OrderDto.Item.Документ_Name} - {OrderDto.Item.Статус}");
                    Return();
                }
                else
                {
                    //await Notification.ShowAsync($"Cтатус изменить не удалось. {Environment.NewLine}{response.ReasonPhrase}", 2);                    
                    await Notification.HideAsync("Cтатус изменить не удалось", 1);
                    await ToBitrixErrors($"Cтатус изменить не удалось: {OrderDto.Item.Документ_Name} - {OrderDto.Item.Статус}");
                }
                Console.WriteLine($"ScannedBarcodeAsync - duration: {DateTime.Now - beginTime}");
            }
            catch (Exception ex)
            {
                await Notification.ShowAsync($"Ошибка изменения статуса. {Environment.NewLine}{ex.Message}", 2);
                Console.WriteLine($"ScannedBarcodeAsync - Exception: {ex.Message}");
                Console.WriteLine($"{ex.StackTrace}");
                await ToBitrixErrors($"Ошибка изменения статуса: {OrderDto.Item.Документ_Name} - {OrderDto.Item.Статус}");
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
