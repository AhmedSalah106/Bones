using Bones_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Bones_App.Services.Implementation
{
    public class PaymentService
    {
        private readonly BonesContext context;
        public PaymentService(BonesContext context)
        {
            this.context = context;
        }

        public async Task SaveTransactionDetailsAsync(string transactionId, decimal amount,  string status, string paymentToken)
        {
            var transaction = new PaymentTransaction
            {
                TransactionId = transactionId,
                Amount = amount,
                PaymentStatus = status,
                PaymentToken = paymentToken,
                TransactionDate = DateTime.UtcNow
            };

            context.PaymentTransactions.Add(transaction);
            await context.SaveChangesAsync();
        }

        public async Task<PaymentTransaction?> GetTransactionByIdAsync(string transactionId)
        {
            return await context.PaymentTransactions
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId);
        }
    }
}
