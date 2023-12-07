using System.Text.Json.Serialization;

namespace Art.Entities
{
    public class Picture: BaseEntity
    {
        public string Name { get; set;}
        public int ArtistId { get; set;}
        [JsonIgnore]
        public virtual Artist Artist { get; set;}
    }
}
