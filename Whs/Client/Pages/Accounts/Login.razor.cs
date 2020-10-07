using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Whs.Client.Identity;
using Whs.Shared.Models.Accounts;

namespace Whs.Client.Pages.Accounts
{
    public partial class Login
    {

        private UserForAuthenticationDto _userForAuthentication = new UserForAuthenticationDto();
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public bool ShowAuthError { get; set; }
        public string Error { get; set; }

        private async Task ScannedBarcode(ChangeEventArgs args)
        {
            _userForAuthentication.Barcode = args.Value.ToString();
            await ExecuteLogin();
        }

        public async Task ExecuteLogin()
        {
            ShowAuthError = false;
            var result = await AuthenticationService.Login(_userForAuthentication);
            if (!result.IsAuthSuccessful)
            {
                Error = result.ErrorMessage;
                ShowAuthError = true;
            }
            else
            {
                NavigationManager.NavigateTo("/");
            }
        }
    }
}
