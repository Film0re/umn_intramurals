using Postgrest.Attributes;
using Postgrest.Models;

namespace umn_supabase.Models
{
    [Table("teams")]
    public class Team
    {
        [PrimaryKey("team_id")]
        [Column("team_id")]
        public Guid team_id { get; set; }

        [Column("name")]
        public string name { get; set; }

        [Column("season")]
        public int season { get; set; }

        [Column("wins")]
        public int wins { get; set; }

        [Column("games_played")]
        public int games_played { get; set; }

        [Column("rank")]
        public int rank { get; set; }
        
    }
}