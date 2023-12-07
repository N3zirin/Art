using System.Text.Json.Serialization;

namespace Art.Entities
{
    public class Artist: BaseEntity
    {
        public string ArtistName { get; set; }
        [JsonIgnore]
        public virtual List<Picture> Pictures { get; set; } // navigation property

    }
}
