using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
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
        [Inject] public IConfiguration Configuration { get; set; }

        string prinDocUrl;

        private string Barcode;
        WhsOrderDtoIn OrderDto;
        public EditingCause[] EditingCauses { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetEditingCausesAsync();
            await GetOrderDtoAsync();
            PrinDocSettings prinDocSettings = Configuration.GetSection(PrinDocSettings.PrinDoc).Get<PrinDocSettings>();
            prinDocUrl = GetPrinDocUrl(prinDocSettings);
        }

        private string GetPrinDocUrl(PrinDocSettings prinDocSettings)
        {
            string page = prinDocSettings.Page;
            string prinDocUrl = "";
            if (page == "Razor")
            {
                prinDocUrl = $"{prinDocSettings.BaseAddress}?service={prinDocSettings.Service}" +
                $"&templateName={prinDocSettings.Values["WhsOrderIn"].Template}" +
                $"&docSource={prinDocSettings.Values["WhsOrderIn"].Endpoint}" +
                $"&id={OrderDto.Item.Документ_Id}";
            }
            else if (page == "Blazor")
            {
                prinDocUrl = $"{prinDocSettings.BaseAddress}{prinDocSettings.Service}/" +
                $"{prinDocSettings.Values["WhsOrderIn"].Template}/" +
                $"{prinDocSettings.Values["WhsOrderIn"].Endpoint}/" +
                $"{OrderDto.Item.Документ_Id}";
            }
            return prinDocUrl;
        }

        private async Task GetEditingCausesAsync()
        {
            EditingCauses = await HttpClient.GetFromJsonAsync<EditingCause[]>("api/EditingCauses/ForIn");
        }

        private async Task GetOrderDtoAsync()
        {
            try
            {
                OrderDto = await HttpClient.GetFromJsonAsync<WhsOrderDtoIn>($"api/WhsOrdersIn/Dto/{Id}", new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await Notification.ShowAsync($"Ошибка загрузки приходного ордера.", 1);
                await ToBitrixErrors($"Ошибка загрузки приходного ордера {Id}: {ex.Message}");
            }
        }

        private async Task ScannedBarcodeAsync(ChangeEventArgs args)
        {
            try
            {
                Notification.Show($"Запрос изменения статуса...");
                Barcode = args.Value.ToString();
                HttpResponseMessage response = await HttpClient.PutAsJsonAsync<WhsOrderIn>($"api/WhsOrdersIn/{OrderDto.Item.Документ_Id}/{Barcode}", OrderDto.Item);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    await GetOrderDtoAsync();
                    await Notification.HideAsync($"{OrderDto.Item.Документ_Name} - {OrderDto.Item.Статус}", 1);
                    if (OrderDto.Item.Статус == WhsOrderStatus.In.AtWork)
                    {
                        await PrintAsync();
                    }
                    Return();
                }
                else
                {
                    await Notification.HideAsync("Cтатус изменить не удалось", 1);
                    await ToBitrixErrors($"Cтатус изменить не удалось: {OrderDto.Item.Документ_Name} {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                await Notification.ShowAsync($"Ошибка изменения статуса.", 1);
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
