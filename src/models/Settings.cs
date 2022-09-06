﻿using Watson.ORM.Core;

namespace TinyScreen.models {
    
    [Table("settings")]
    public class Settings {
        
        [Column("id", true, DataTypes.Int, false)]
        public int Id { get; set; }
        
        [Column("name", false, DataTypes.Nvarchar, 64, false)]
        public string Name { get; set; }
        
        [Column("value", false, DataTypes.Blob, 64, false)]
        public string Value { get; set; }
    }
}