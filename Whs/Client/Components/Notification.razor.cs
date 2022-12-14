using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Whs.Client.Components
{
    public partial class Notification
    {
        [Parameter]
        public string Message { get; set; }

        [Parameter]
        public RenderFragment RenderFragment { get; set; }


        public void Show(string message)
        {
            Message = message;
            _modalDisplay = "block;";
            _modalClass = "show";
            _showBackdrop = true;
            StateHasChanged();
        }

        public void Hide()
        {
            _modalDisplay = "none;";
            _modalClass = "";
            _showBackdrop = false;
            StateHasChanged();
        }

        public async Task ShowAsync(string message, int delay)
        {
            Show(message);
            await Task.Delay(1000 * delay);
            Hide();
        }

        public async Task HideAsync(string message, int delay)
        {
            Message = message;
            StateHasChanged();
            await Task.Delay(1000 * delay);
            Hide();
        }
    }
}
