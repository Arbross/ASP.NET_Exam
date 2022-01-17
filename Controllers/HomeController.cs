using Exam_ASP_NET.Models;
using Exam_ASP_NET.Models.ViewModels;
using Exam_ASP_NET.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_ASP_NET.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext _context;

        public HomeController(DatabaseContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Details(int id)
        {
            Purchase purchase = _context.Purchases.Find(id);
            if (purchase == null) return NotFound();

            _context.Entry(purchase).Reference(nameof(Purchase.Category)).Load();

            bool isAddedToCart = false;
            List<ShoppingProduct> products = HttpContext.Session.GetObject<List<ShoppingProduct>>(WebConstants.CartKey);
            if (products != null)
            {
                if (products.FirstOrDefault(i => i.ProductId == id) != null)
                {
                    isAddedToCart = true;
                }  
            }

            return View(new DetailsVM() { Purchase = purchase, IsAddedToCart = isAddedToCart });
        }

        public IActionResult AddToCart(int id)
        {
            List<ShoppingProduct> products = HttpContext.Session.GetObject<List<ShoppingProduct>>(WebConstants.CartKey);
            if (products == null)
            {
                products = new List<ShoppingProduct>();
            }

            products.Add(new ShoppingProduct() { ProductId = id });
            HttpContext.Session.SetObject(WebConstants.CartKey, products);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            List<ShoppingProduct> products = HttpContext.Session.GetObject<List<ShoppingProduct>>(WebConstants.CartKey);
            if (products != null)
            {
                products.Remove(products.FirstOrDefault(i => i.ProductId == id));
            }

            HttpContext.Session.SetObject(WebConstants.CartKey, products);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
        {
            HomeVM viewModel = new HomeVM()
            {
                Purchases = _context.Purchases.ToList()
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
