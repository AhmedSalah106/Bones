using System.Text.Json.Serialization;

namespace Bones_App.DTOs
{
    public class PaymobWebhookPayload
    {
        [JsonPropertyName("obj")]
        public PaymobTransaction Obj { get; set; }
    }
}
