using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Xml.Linq;

namespace Client.Controllers
{
    public class StripeController : Controller
    {

        private readonly ILogger<StripeController> _logger;

        public StripeController(ILogger<StripeController> logger)
        {
            _logger = logger;
          
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentIntents()
        {
            int amount = 2;
            StripeConfiguration.ApiKey = "sk_test_51NgBVDEPVS0RJPUBKZmvKus2rWpb7O1wJgHYR0qLL8mSBCPMmQey1lFGOQtUEgzTmwO3a6EwdqGVhUK31GIm8lRl00s9r92m01";

            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "usd",
                };
                var service = new PaymentIntentService();
                var paymentIntent = await service.CreateAsync(options);

                return Ok(new { ClientSecret = paymentIntent.ClientSecret });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }


        string DomainName = "https://localhost:7258/", sessionId;

        [HttpPost]
        public IActionResult CreatePaymentIntent()
        {

            var Name = "shabeer";
            var Email = "shabir@gmail.com";
            int amount = 2;
            
            //    Role = v2;
            //    CreatedDate = DateTime.Now;
            StripeConfiguration.ApiKey = "sk_test_51NgBVDEPVS0RJPUBKZmvKus2rWpb7O1wJgHYR0qLL8mSBCPMmQey1lFGOQtUEgzTmwO3a6EwdqGVhUK31GIm8lRl00s9r92m01";


            var options = new PriceCreateOptions
            {
                UnitAmount = amount, // Convert to cents
                Currency = "usd",
                ProductData = new PriceProductDataOptions
                {
                    Name = Name
                }
            };

            var priceService = new PriceService();
            Price price = priceService.Create(options);

            // Create a new checkout session with the price as the line item
            var sessionOptions = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = price.Id,
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = DomainName + "PaymentConfirmation?Name=" + Name + "&Email=" + Email + "&Amount=" + amount + "",
                CancelUrl = DomainName + "Index",
            };


            var service = new SessionService();
            var session = service.Create(sessionOptions);
            sessionId = session.Id;
            return Redirect(session.Url);
        }

     
    }
}
