using Bones_App.DTOs;
using Bones_App.Models;
using Bones_App.Repositories.Interfaces;
using Bones_App.Services.Interfaces;
using Bones_App.Services.SharedService;

namespace Bones_App.Services.Implementation
{
    public class PaymentTransactionService:Service<PaymentTransaction>,IpaymentTransactionService
    {
        private readonly IPaymentTransactionRepository paymentTransactionRepository;
        public PaymentTransactionService(IPaymentTransactionRepository paymentTransactionRepository):base(paymentTransactionRepository) 
        {
            this.paymentTransactionRepository = paymentTransactionRepository;
        }

        public PaymentTransactionResponseDTO ConvertFromPaymentTransactionToPaymentTransactionResponseDTO(PaymentTransaction paymentTransaction)
        {
            PaymentTransactionResponseDTO payment = new PaymentTransactionResponseDTO()
            {
                PaymentStatus=paymentTransaction.PaymentStatus,
                TransactionDate=paymentTransaction.TransactionDate,
                Amount=paymentTransaction.Amount,
                PaymentToken=paymentTransaction.PaymentToken,
                TransactionId=paymentTransaction.TransactionId
            };

            return payment;
        }

        public List<PaymentTransactionResponseDTO> ConvertFromPaymentTransactionToPaymentTransactionResponseDTOList(List<PaymentTransaction> paymentTransactions)
        {
            List<PaymentTransactionResponseDTO> paymentTransactionsDTOs = paymentTransactions.Select(pay => ConvertFromPaymentTransactionToPaymentTransactionResponseDTO(pay)).ToList();
            return paymentTransactionsDTOs;
        }

        public decimal GetTotalTransactionPayments()
        {
            decimal Total = paymentTransactionRepository.GetAll().Select(pay=>pay.Amount).Sum();

            return Total;
        }
    }
}
