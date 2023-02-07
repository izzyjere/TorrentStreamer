using System.ComponentModel.DataAnnotations;

namespace TorrentStreamer;

public class TorrentFile
{
    [Key]
    public  Guid Id { get; set; }  
    public string FileName { get; set; }  
    public double FileSize { get; set; }
    public DateTime DateAdded { get; set; }

}
