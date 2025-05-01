using System.Text.Json.Serialization;

namespace Bones_App.DTOs
{
    public class PaymobPaymentKeyClaims
    {
        [JsonPropertyName("billing_data")]
        public PaymobBillingData BillingData { get; set; }
    }

    public class PaymobBillingData
    {
        [JsonPropertyName("extra_description")]
        public string ExtraDescription { get; set; }

        [JsonPropertyName("first_name")]
        public string Nat_Id { get; set; }
    }
}
