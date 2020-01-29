using System.Text.Json.Serialization;

namespace Bookshelf.Core
{
    public class OpenLibraryData
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("number_of_pages")]
        public int PageCount { get; set; }
        [JsonPropertyName("cover")]
        public Cover Covers { get; set; }
        [JsonPropertyName("authors")]
        public Author[] Authors { get; set; }
    }

    public class Cover
    {
        [JsonPropertyName("medium")]
        public string Url { get; set; }
    }

    public class Author
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}