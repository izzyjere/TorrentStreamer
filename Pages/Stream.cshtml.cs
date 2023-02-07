using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace TorrentStreamer.Pages
{
    public class StreamModel : PageModel
    {
        readonly TorrentContext db;
        readonly IWebHostEnvironment _env;
        public StreamModel(TorrentContext _db, IWebHostEnvironment env)
        {
             db = _db;
            _env = env;
        }
        public TorrentFile TorrentFile { get; set; } = new();
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if(id == Guid.Empty)
            {
                return NotFound();
            }
            var record = await db.TorrentFiles.FirstOrDefaultAsync(f=> f.Id == id); 
            if(record == null)
            {
                return NotFound();
            }
            TorrentFile = record;   
            return Page();
        }
     
        
    }
}
