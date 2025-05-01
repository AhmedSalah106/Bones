using Bones_App.DTOs;
using Bones_App.Models;
using Bones_App.Services.SharedService;

namespace Bones_App.Services.Interfaces
{
    public interface IpaymentTransactionService:IService<PaymentTransaction>
    {
        decimal GetTotalTransactionPayments();
        PaymentTransactionResponseDTO ConvertFromPaymentTransactionToPaymentTransactionResponseDTO(PaymentTransaction paymentTransaction);

        List<PaymentTransactionResponseDTO>ConvertFromPaymentTransactionToPaymentTransactionResponseDTOList(List<PaymentTransaction>paymentTransactions);
    }
}
