using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Whs.Client.Identity;
using Whs.Shared.Models;
using Whs.Shared.Models.Accounts;

namespace Whs.Client.Pages.Accounts
{
    public partial class Registration
    {
        private UserForRegistrationDto _userForRegistration = new UserForRegistrationDto();

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public HttpClient _httpClient { get; set; }

        public bool ShowRegistrationErrors { get; set; }

        public IEnumerable<string> Errors { get; set; }

        private List<Warehouse> Warehouses = new List<Warehouse>();

        protected override async Task OnInitializedAsync()
        {
            await GetWarehousesAsync();
        }
        private async Task GetWarehousesAsync()
        {
            Warehouses = await _httpClient.GetFromJsonAsync<List<Warehouse>>("api/Warehouses");
        }

        public async Task Register()
        {
            ShowRegistrationErrors = false;
            var result = await AuthenticationService.RegisterUser(_userForRegistration);

            if (!result.IsSuccessfulRegistration)
            {
                Errors = result.Errors;
                ShowRegistrationErrors = true;
            }
            else
            {
                NavigationManager.NavigateTo("/Users");
            }
        }
    }
}
