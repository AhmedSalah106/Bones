using System.Text.Json.Serialization;

namespace Bones_App.DTOs
{
    public class PaymobOrder
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
    }
}
