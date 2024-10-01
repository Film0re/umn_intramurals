using Postgrest.Attributes;
using Postgrest.Models;

namespace umn_supabase.Models
{
    [Table("matches")]
    public class Match : BaseModel
    {
        [PrimaryKey("match_id")]
        [Column("match_id")]
        public string match_id { get; set; }
        
        [Column("match_length")]
        public int match_length { get; set; }

        [Column("season")]
        public int season { get; set; }
        
        [Column("game_version")]
        public string game_version { get; set; }
        
        [Column("team1_id")]
        public Guid? team1_id { get; set; }
        
        [Column("team2_id")]
        public Guid? team2_id { get; set; }
        
        [Column("winner_team_id")]
        public Guid? winner_team_id { get; set; }
        
        [Column("is_playoffs")]
        public bool is_playoffs { get; set; } = false;
        
        [Column("week")]
        public int week { get; set; }

    }
}