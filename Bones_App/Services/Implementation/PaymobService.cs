using Bones_App.DTOs;
using Microsoft.AspNetCore.Authentication.OAuth;

public class PaymobService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public PaymobService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<string> GetAuthTokenAsync()
    {
        var response = await _httpClient.PostAsJsonAsync("https://accept.paymob.com/api/auth/tokens", new
        {
            api_key = _config["Paymob:ApiKey"]
        });

        var content = await response.Content.ReadFromJsonAsync<AuthTokenResponse>();
        return content.token;
    }

    public async Task<int> CreateOrderAsync(string token, decimal amountCents)
    {
        var response = await _httpClient.PostAsJsonAsync("https://accept.paymob.com/api/ecommerce/orders", new
        {
            auth_token = token,
            delivery_needed = false,
            amount_cents = (int)(amountCents * 100),
            currency = "EGP",
            items = new object[] { }
        });

        var content = await response.Content.ReadFromJsonAsync<OrderResponse>();
        return content.id;
    }

    public async Task<string> GetPaymentKeyAsync(string token, int orderId, decimal amountCents, string email, string phone)
    {
        var response = await _httpClient.PostAsJsonAsync("https://accept.paymob.com/api/acceptance/payment_keys", new
        {
            auth_token = token,
            amount_cents = (int)(amountCents * 100),
            expiration = 3600,
            order_id = orderId,
            billing_data = new
            {
                apartment = "NA",
                email = email,
                floor = "NA",
                first_name = "Ahmed",
                street = "NA",
                building = "NA",
                phone_number = phone,
                shipping_method = "NA",
                postal_code = "NA",
                city = "NA",
                country = "NA",
                last_name = "Salah",
                state = "NA"
            },
            currency = "EGP",
            integration_id = int.Parse(_config["Paymob:IntegrationId"])
        });

        var content = await response.Content.ReadFromJsonAsync<PaymentKeyResponse>();
        return content.token;
    }
}
