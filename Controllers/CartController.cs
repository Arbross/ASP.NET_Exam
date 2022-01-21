using Exam_ASP_NET.Models;
using Exam_ASP_NET.Models.ViewModels;
using Exam_ASP_NET.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_ASP_NET.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly DatabaseContext _context;

        public CartController(DatabaseContext context)
        {
            _context = context;
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

        [AllowAnonymous]
        public IActionResult Index()
        {
            List<ShoppingProduct> products = HttpContext.Session.GetObject<List<ShoppingProduct>>(WebConstants.CartKey);

            List<Purchase> purchases = new List<Purchase>();

            foreach (var item in products)
            {
                purchases.Add(_context.Purchases.Include(nameof(Models.Purchase.Category)).FirstOrDefault(x => item.ProductId == x.Id));
            }

            CartVM viewModel = new CartVM()
            {
                Purchases = purchases
            };

            return View(viewModel);
        }

        public IActionResult Confirm()
        {
            return View();
        }
    }
}
