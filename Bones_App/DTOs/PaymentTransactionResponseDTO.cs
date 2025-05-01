namespace Bones_App.DTOs
{
    public class PaymentTransactionResponseDTO
    {
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentToken { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
