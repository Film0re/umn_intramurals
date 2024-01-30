using Postgrest.Attributes;
using Postgrest.Models;

namespace umn_supabase.Models{

    [Table("team_players")]
    public class TeamPlayer : BaseModel
    {
        [PrimaryKey("team_id")]
        [Column("team_id")]
        public Guid team_id { get; set; }
        
        [PrimaryKey("player_id")]
        [Column("player_id")]
        public string player_id { get; set; }
        
        
        [Column("created_at")]
        public DateTime created_at { get; set; }
        
        [Column("intramural_season")]
        public int intramural_season { get; set; }

    }
}