using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace WebApplicationWebApi_Arooj.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StripeController : ControllerBase
    {
        private readonly string _stripeSecretKey = "sk_test_51NgBVDEPVS0RJPUBKZmvKus2rWpb7O1wJgHYR0qLL8mSBCPMmQey1lFGOQtUEgzTmwO3a6EwdqGVhUK31GIm8lRl00s9r92m01"; // Replace with your Stripe secret key

        public StripeController()
        {
            StripeConfiguration.ApiKey = _stripeSecretKey;
        }

        [HttpPost("create-payment-intent")]
        public IActionResult CreatePaymentIntent([FromBody] CreatePaymentIntentRequest request)
        {
            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = request.Amount, 
                    Currency = "usd", // Change to your desired currency
                };

                var service = new PaymentIntentService();
                var paymentIntent = service.Create(options);

                return Ok(new { ClientSecret = paymentIntent.ClientSecret });
            }
            catch (StripeException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }
    }

    public class CreatePaymentIntentRequest
    {
        public long Amount { get; set; }
    }
}
