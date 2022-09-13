﻿using Watson.ORM.Core;

namespace TinyScreen.Models {
    [Table("games")]
    public class Games {
        
        [Column("id", true, DataTypes.Int, false)]
        public int Id { get; set; }
        
        [Column("sourceId", false, DataTypes.Int, false)]
        public int SourceId { get; set; }
        
        [Column("name", false, DataTypes.Nvarchar, 128, false)]
        public string Name { get; set; }
        
        [Column("description", false, DataTypes.Nvarchar, 512, true)]
        public string Description { get; set; }
        
        [Column("artwork", false, DataTypes.Nvarchar, 255, false)]
        public string Artwork { get; set; }
        
        [Column("background", false, DataTypes.Nvarchar, 255, false)]
        public string Background { get; set; }
        
        [Column("source", false, DataTypes.Int, 2, false)]
        public int Source { get; set; }
        
        [Column("lastPlayed", false, DataTypes.DateTime, 2, false)]
        public string LastPlayed { get; set; }
        
    }
}