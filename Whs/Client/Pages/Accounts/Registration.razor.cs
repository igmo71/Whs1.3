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
            await GetСкладыAsync();
        }
        private async Task GetСкладыAsync()
        {
            Warehouses = await _httpClient.GetFromJsonAsync<List<Warehouse>>("api/Warehouses");
        }

        public async Task Register()
        {
            ShowRegistrationErrors = false;
            if (string.IsNullOrEmpty(_userForRegistration.Password))
                _userForRegistration.Password = "1qaz@WSX";
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
