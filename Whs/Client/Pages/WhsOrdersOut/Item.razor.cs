using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Whs.Client.Components;
using Whs.Shared.Models;

namespace Whs.Client.Pages.WhsOrdersOut
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
        WhsOrderDtoOut OrderDto;
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
                $"&templateName={prinDocSettings.Values["WhsOrderOut"].Template}" +
                $"&docSource={prinDocSettings.Values["WhsOrderOut"].Endpoint}" +
                $"&id={OrderDto.Item.Документ_Id}";
            }
            else if (page == "Blazor")
            {
                prinDocUrl = $"{prinDocSettings.BaseAddress}{prinDocSettings.Service}/" +
                $"{prinDocSettings.Values["WhsOrderOut"].Template}/" +
                $"{prinDocSettings.Values["WhsOrderOut"].Endpoint}/" +
                $"{OrderDto.Item.Документ_Id}";
            }
            return prinDocUrl;
        }

        private async Task GetEditingCausesAsync()
        {
            EditingCauses = await HttpClient.GetFromJsonAsync<EditingCause[]>("api/EditingCauses/ForOut");
        }

        private async Task GetOrderDtoAsync()
        {
            try
            {
                OrderDto = await HttpClient.GetFromJsonAsync<WhsOrderDtoOut>($"api/WhsOrdersOut/Dto/{Id}", new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await Notification.ShowAsync($"Ошибка загрузки расходного ордера.", 1);
                await ToBitrixErrors($"Ошибка загрузки расходного ордера {Id}: {ex.Message}");
            }
        }

        private async Task ScannedBarcodeAsync(ChangeEventArgs args)
        {
            try
            {
                Notification.Show($"Запрос изменения статуса...");
                Barcode = args.Value.ToString();
                HttpResponseMessage response = await HttpClient.PutAsJsonAsync<WhsOrderOut>($"api/WhsOrdersOut/{OrderDto.Item.Документ_Id}/{Barcode}", OrderDto.Item);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    await GetOrderDtoAsync();
                    await Notification.HideAsync($"{OrderDto.Item.Документ_Name} - {OrderDto.Item.Статус}", 1);
                    if (OrderDto.Item.Статус == WhsOrderStatus.Out.ToCollect)
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
            NavigationManager.NavigateTo($"WhsOrdersOut/CardsByQueType/{SearchStatus}");
        }

        private async Task ToBitrixErrors(string message)
        {
            await HttpClient.PostAsync($"api/ToBitrixErrors/{message}", null);
        }
    }
}
