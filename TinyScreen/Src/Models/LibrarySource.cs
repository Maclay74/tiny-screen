using System.Collections.Generic;

namespace TinyScreen.Models; 

public class LibrarySource {
        
    public int Id { get; set; }
        
    public string Name { get; set; }
        
    public int GamesCount { get; set; }

    public ICollection<Game>? Games { get; set; } = null;
}
