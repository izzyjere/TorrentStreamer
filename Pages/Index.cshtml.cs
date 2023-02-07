using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TorrentStreamer.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
    public async Task<IActionResult> OnPostUploadAsync(IFormFile torrentFile)
    {
        if (torrentFile == null || torrentFile.Length == 0)
        {
            ModelState.AddModelError("torrentFile", "No torrent file was uploaded");
            return Page();
        }

        // Save the torrent file to the server
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "torrents", torrentFile.FileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await torrentFile.CopyToAsync(fileStream);
        }

        return Page();
    }

}
