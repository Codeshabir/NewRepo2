using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe;
using System.Diagnostics;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<StripeController> _logger;

        private readonly HouseService _houseService;

        public HomeController(HouseService houseService, ILogger<StripeController> logger)
        {
            _houseService = houseService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        
        {
            var houses = await _houseService.GetHousesAsync();
            foreach (var house in houses)
            {
                Debug.WriteLine($"House ID: {house.Id}");
            }
            return View(houses);
        }
        
        public async Task<IActionResult> About()
        
        {
            return View();
        }


        public async Task<IActionResult> Contact()

        {
            return View();
        }


        public async Task<IActionResult> Details(int id)
        {
            var house = await _houseService.GetHouseAsync(id);
            if (house == null)
            {
                return NotFound();
            }
            return View(house);
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
