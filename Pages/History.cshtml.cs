using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace TorrentStreamer.Pages
{
    public class HistoryModel : PageModel
    {
        TorrentContext db;
        public HistoryModel(TorrentContext _db)
        {
            db = _db;
        }
        public IEnumerable<TorrentFile> TorrentFiles { get; set; } = Enumerable.Empty<TorrentFile>();

        public async Task<IActionResult> OnGetAsync()
        {
            TorrentFiles = await db.TorrentFiles.ToListAsync();
            return Page();
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

    }
}
