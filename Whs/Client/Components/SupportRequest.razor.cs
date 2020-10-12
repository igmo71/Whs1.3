using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Whs.Client.Components
{
    public partial class SupportRequest
    {
        [Parameter]
        public string Message { get; set; }

        [Parameter]
        public RenderFragment RenderFragment { get; set; }

        private string _modalDisplay;
        private string _modalClass;
        private bool _showBackdrop;

        public void Show(string message)
        {
            Message = message;
            _modalDisplay = "block;";
            _modalClass = "show";
            _showBackdrop = true;
            StateHasChanged();
        }

        public async Task ShowAsync(string message, int delay)
        {
            Show(message);
            await Task.Delay(1000 * delay);
            Hide();
        }
        public void Hide()
        {
            _modalDisplay = "none;";
            _modalClass = "";
            _showBackdrop = false;
            StateHasChanged();
        }
    }
}
