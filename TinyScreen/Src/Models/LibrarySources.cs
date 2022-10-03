using Watson.ORM.Core;

namespace TinyScreen.Models {
    
    [Table("library-sources")]
    public partial class LibrarySources {
        
        [Column("id", true, DataTypes.Int, false)]
        public int Id { get; set; }
        
        [Column("name", false, DataTypes.Nvarchar, 128, false)]
        public string Name { get; set; }
        
        [Column("gamesCount", false, DataTypes.Int, 5, false)]
        public int GamesCount { get; set; }
    }
}