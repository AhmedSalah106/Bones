using System.Text.Json.Serialization;

namespace Bones_App.DTOs
{
    public class PaymobTransaction
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("amount_cents")]
        public int AmountCents { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("payment_key_claims")]
        public PaymobPaymentKeyClaims PaymentKeyClaims { get; set; }

        [JsonPropertyName("order")]
        public PaymobOrder Order { get; set; }

        [JsonPropertyName("source_data")]
        public PaymobSourceData SourceData { get; set; }
    }

}
