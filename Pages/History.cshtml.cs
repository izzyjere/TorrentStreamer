using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TorrentStreamer.Pages
{
    public class HistoryModel : PageModel
    {
        public IEnumerable<FileInfo> TorrentFiles { get; set; }

        public void OnGet()
        {
            var torrentsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "torrents");
            var directoryInfo = new DirectoryInfo(torrentsDirectory);
            TorrentFiles = directoryInfo.GetFiles().Where(f=>f.Extension==".torrent");
        }

    }
}
