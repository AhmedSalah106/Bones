namespace Bones_App.Models
{
    public class PaymentTransaction
    {

        public int Id { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount {  get; set; }
        public string PaymentStatus {  get; set; }
        public string PaymentToken {  get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
