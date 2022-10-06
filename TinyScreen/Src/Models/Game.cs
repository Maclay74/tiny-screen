using System;

namespace TinyScreen.Models; 

public class Game {
        
    public int Id { get; set; }

    public string OriginalId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
    public string Artwork { get; set; }

    public string Background { get; set; }

    public LibrarySource Source { get; set; }
        
    public DateTime? LastPlayed { get; set; }
}