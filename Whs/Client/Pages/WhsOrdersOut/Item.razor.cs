using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Whs.Client.Components;
using Whs.Shared.Models;

namespace Whs.Client.Pages.WhsOrdersOut
{
    public partial class Item
    {
        [Parameter]
        public string Id { get; set; }
        [Inject]
        HttpClient HttpClient { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private string Barcode;
        private Notification Notification;
        WhsOrderDtoOut OrderDto;
        //public EditingCause[] EditingCause { get; set; }


        protected override async Task OnInitializedAsync()
        {
            //await GetEditingReasonsAsync();
            await GetOrderDtoAsync();
        }

        //private async Task GetEditingReasonsAsync()
        //{
        //    EditingCause = await HttpClient.GetFromJsonAsync<EditingCause[]>("api/EditingReasons/getOuts");
        //}

        private async Task GetOrderDtoAsync()
        {
            try
            {
                DateTime beginTime = DateTime.Now;
                Console.WriteLine("GetOrderDtoAsync - begin");
                OrderDto = await HttpClient.GetFromJsonAsync<WhsOrderDtoOut>($"api/WhsOrdersOut/Dto/{Id}");
                Console.WriteLine($"GetOrderDtoAsync - duration: {DateTime.Now - beginTime}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"GetOrderDtoAsync Excepton: {Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}");
                await Notification.ShowAsync($"Ошибка{Environment.NewLine}{ex.Message}", 2);
            }
        }

        private async Task ScannedBarcodeAsync(ChangeEventArgs args)
        {
            DateTime beginTime = DateTime.Now;
            Console.WriteLine("ScannedBarcodeAsync - begin");
            Barcode = args.Value.ToString();
            Console.WriteLine($"ScannedBarcodeAsync - Barcode: {Barcode}");
            try
            {
                Console.WriteLine($"ScannedBarcodeAsync - requestUri: {$"api/WhsOrdersOut/{OrderDto.Item.Документ_Id}/{Barcode}"}");
                HttpResponseMessage response = await HttpClient.PutAsJsonAsync<WhsOrderOut>($"api/WhsOrdersOut/{OrderDto.Item.Документ_Id}/{Barcode}", OrderDto.Item);
                Console.WriteLine($"HttpResponseMessage - response: {response.StatusCode} - {response.ReasonPhrase} - {await response.Content.ReadAsStringAsync()}");
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    if (OrderDto.IsAutoPrint)
                    {
                        DateTime beginTimePrint = DateTime.Now;
                        await GetOrderDtoAsync();
                        StateHasChanged();
                        Console.WriteLine("GetOrderDtoAsync Print - begin");
                        await PrintAsync();
                        Console.WriteLine($"GetOrderDtoAsync Print - duration: {DateTime.Now - beginTimePrint}");
                    }
                    NavigationManager.NavigateTo("/WhsOutsByQueType");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ScannedBarcodeAsync - Exception: {ex.Message}");
                await Notification.ShowAsync($"Ошибка изменения статуса{Environment.NewLine}{ex.Message}", 3);
            }
            Console.WriteLine($"ScannedBarcodeAsync - duration: {DateTime.Now - beginTime}");
        }


        private async Task PrintAsync() => await JSRuntime.InvokeVoidAsync("print");
    }
}
