using System.Collections.Generic;

namespace TinyScreen.Models; 

public class Folder {
        
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Artwork { get; set; }

    public ICollection<Game> Games { get; set; }
}