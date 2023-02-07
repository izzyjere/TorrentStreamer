using Microsoft.EntityFrameworkCore;

namespace TorrentStreamer;

public class TorrentContext : DbContext
{
	public TorrentContext(DbContextOptions<TorrentContext> options):base(options)
	{

	}
	public DbSet<TorrentFile> TorrentFiles { get; set; }
}
