using System.Text.Json.Serialization;

namespace Bookshelf.DataFetcher
{
    public class OpenLibraryDetails
    {
        [JsonPropertyName("details")]
        public Details Details { get; set; }
    }

    public class Details
    {
        [JsonPropertyName("description")]
        public Description Description { get; set; }     
    }

    public class Description
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}