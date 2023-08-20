using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Xml.Linq;

namespace Client.Controllers
{
    //[Route("api/checkout")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly string _stripeSecretKey;

        public CheckoutController(IConfiguration configuration)
        {
            _stripeSecretKey = configuration["Stripe:SecretKey"];
        }
        [Route("api/Checkout/PayWithStripe")]
        [HttpGet]
        public JsonResult PayWithStripe(int amount)
        {
            StripeConfiguration.ApiKey = _stripeSecretKey;

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
            {
                "card"
            },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = amount, // amount in cents
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Payment Integrate"
                        }
                    },
                    Quantity = 1
                }
            },
                Mode = "payment",
                SuccessUrl = "https://your-website.com/success",
                CancelUrl = "https://your-website.com/cancel"
            };

            var service = new SessionService();
            var session = service.Create(options);
            return new JsonResult(new { URL = session.Url, SuccessURL = session.SuccessUrl, CancelURL = session.CancelUrl });
            //return Ok(new { sessionId = session.Id });
        }
    }
}
