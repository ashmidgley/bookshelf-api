using System.Text.Json.Serialization;

namespace Api
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