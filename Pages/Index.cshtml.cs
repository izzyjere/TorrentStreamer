using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace TorrentStreamer.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly TorrentContext db;
    public IEnumerable<TorrentFile> TorrentFiles { get; set; } = Enumerable.Empty<TorrentFile>();

    public IndexModel(ILogger<IndexModel> logger, TorrentContext db)
    {
        _logger = logger;
        this.db = db;
    }
    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        var file = await db.TorrentFiles.FirstOrDefaultAsync(t => t.Id == id);
        if (file == null)
        {
            return Page();
        }
        else
        {
            db.TorrentFiles.Remove(file);
            var done = await db.SaveChangesAsync();
            if (done != 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "torrents", file.FileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            return Page();
        }

    }
    public async Task<IActionResult> OnGetAsync()
    {
        TorrentFiles = await db.TorrentFiles.ToListAsync();
        return Page();
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
