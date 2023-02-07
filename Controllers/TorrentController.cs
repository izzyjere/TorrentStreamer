using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TorrentStreamer.Controllers;
[ApiController]
[Route("[controller]")]
public class TorrentController : ControllerBase
{
    readonly TorrentContext db;
    readonly IWebHostEnvironment _env;
    public TorrentController(TorrentContext _db, IWebHostEnvironment env)
    {
        db = _db;
        _env = env;
    }
    [HttpGet]
    public async Task<IActionResult> OnGetFetchTorrent([FromQuery] Guid id)
    {
        if (id == Guid.Empty)
        {
            Console.WriteLine("Not found.");
            return NotFound();
        }
        var record = await db.TorrentFiles.FirstOrDefaultAsync(f => f.Id == id);
        if (record == null)
        {
            return NotFound();
        }

        var filePath = Path.Combine(_env.WebRootPath, "torrents", record.FileName);
        return PhysicalFile(filePath, "application/x-bittorrent");
    }


}
