﻿using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
