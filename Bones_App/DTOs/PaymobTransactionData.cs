using Bones_App.DTOs;
using System.Text.Json.Serialization;

public class PaymobTransactionData
{
    [JsonPropertyName("billing")]
    public PaymobBillingData Billing { get; set; }
}