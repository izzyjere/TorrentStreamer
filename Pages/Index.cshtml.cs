using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TorrentStreamer.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly TorrentContext db;
    public IndexModel(ILogger<IndexModel> logger, TorrentContext db)
    {
        _logger = logger;
        this.db = db;
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
        var file = new TorrentFile()
        {
            FileName = torrentFile.FileName,           
            FileSize = torrentFile.Length,
            DateAdded = DateTime.Now
        };
        db.TorrentFiles.Add(file);
        await db.SaveChangesAsync();
        return Page();
    }

}
