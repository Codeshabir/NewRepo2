using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe;
using System.Diagnostics;
using Client.Models;
using System.Net.Http;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<CheckoutController> _logger;

        private readonly HouseService _houseService;
        private readonly HttpClient _httpClient;


        public HomeController(HouseService houseService, ILogger<CheckoutController> logger, HttpClient httpClient)
        {
            _houseService = houseService;
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7118"); // Replace with your Web API URL

        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var houses = await _houseService.GetHousesAsync();
            var categories = await _httpClient.GetAsync("/api/Houses/categories");

            if (categories.IsSuccessStatusCode)
            {
                var categoryList = await categories.Content.ReadFromJsonAsync<List<Category>>();
                ViewBag.Categories = categoryList;
            }

            if (categoryId.HasValue && categoryId > 0)
            {
                houses = houses.Where(h => h.CategoryId == categoryId).ToList();
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
