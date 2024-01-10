using Postgrest.Attributes;
using Postgrest.Models;
using umn_supabase;

namespace umn_supabase.Models
{
    [Table("players")]
    public class Player : BaseModel
    {
        [PrimaryKey("player_id")]
        [Column("player_id")]
        public string player_id { get; set; }

        [Column("user_id")]
        public Guid? user_id { get; set; }

        [Column("name")]
        public string name { get; set; }


    }
}
