using System.Text.Json.Serialization;

namespace Bones_App.DTOs
{
    public class PaymobSourceData
    {
        [JsonPropertyName("sub_type")]
        public string SubType { get; set; }
    }
}
