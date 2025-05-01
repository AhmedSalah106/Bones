using Bones_App.DTOs;
using Bones_App.Services.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bones_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymobService _paymobService;
        private readonly IConfiguration configuration;
        private readonly PaymentService paymentService;
        public PaymentController(PaymentService paymentService,PaymobService paymobService, IConfiguration configuration)
        {
            _paymobService = paymobService;
            this.configuration = configuration;
            this.paymentService = paymentService;
        }

        [HttpPost("pay")]
        public async Task<IActionResult> Pay([FromBody] PayRequestModel model)
        {
            var token = await _paymobService.GetAuthTokenAsync();
            var orderId = await _paymobService.CreateOrderAsync(token, model.Amount);
            var paymentKey = await _paymobService.GetPaymentKeyAsync(token, orderId, model.Amount, model.Email, model.Phone);

            string iframeUrl = $"https://accept.paymob.com/api/acceptance/iframes/{configuration["Paymob:IframeId"]}?payment_token={paymentKey}";

            return Ok(new { iframeUrl });
        }


        [HttpPost("webhook")]
        public async Task<IActionResult> HandleWebhook([FromBody] PaymobWebhookPayload payload)
        {
            if (payload?.Obj == null)
                return BadRequest();

            // Check if the payment was successful
            if (payload.Obj.Success)
            {
                await paymentService.SaveTransactionDetailsAsync(
                    payload.Obj.Id.ToString(),
                    payload.Obj.AmountCents / 100m,
                    "success",
                    "sss"
                );
            }
            else
            {
                // Log or handle failed payment
                await paymentService.SaveTransactionDetailsAsync(
                    payload.Obj.Id.ToString(),
                    payload.Obj.AmountCents / 100m,
                    "failed",
                    "sss"
                );
            }

            return Ok(); // Must return 200 to tell Paymob you received it
        }


        
    }
}
