using Bones_App.Models;
using Bones_App.Repositories.Interfaces;
using Bones_App.Repositories.SharedRepo;

namespace Bones_App.Repositories.Implementation
{
    public class PaymentTransactionRepository:Repository<PaymentTransaction>,IPaymentTransactionRepository
    {
        private readonly BonesContext context;
        public PaymentTransactionRepository(BonesContext context):base(context) 
        {
            this.context = context;
        }
    }
}
