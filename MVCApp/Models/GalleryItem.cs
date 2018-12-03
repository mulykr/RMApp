using Newtonsoft.Json;

namespace MVCApp.Models
{
    public class GalleryItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        [JsonProperty(PropertyName = "authorId")]
        public string AuthorId { get; set; }

        [JsonProperty(PropertyName = ("isPublic"))]
        public bool? IsPublic { get; set; }
    }
}