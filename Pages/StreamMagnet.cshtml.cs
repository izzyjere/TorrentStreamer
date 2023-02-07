using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TorrentStreamer.Pages
{
    public class StreamMagnetModel : PageModel
    {
        public string MagnetUrl { get; set; }
        public void OnGet([FromQuery] string handler)
        {
            MagnetUrl = handler;
        }
    }
}
