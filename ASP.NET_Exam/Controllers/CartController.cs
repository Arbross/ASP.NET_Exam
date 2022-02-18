using Exam_ASP_NET.Models;
using Exam_ASP_NET.RazorService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Exam_ASP_NET.Utilities;
using static Exam_ASP_NET.Utilities.SessionExtensions;
using Microsoft.EntityFrameworkCore;
using ReplicaData.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Exam_ASP_NET.ViewModels;

namespace Exam_ASP_NET.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly IViewRender _viewRender;

        public CartController(DatabaseContext context, IEmailSender emailSender, IViewRender viewRender)
        {
            _unitOfWork = new UnitOfWork(context);
            _emailSender = emailSender;
            _viewRender = viewRender;
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
            CartVM viewModel = new CartVM()
            {
                Purchases = GetPurchasesFromSession()
            };

            return View(viewModel);
        }

        public IActionResult Receipt()
        {
            ReceiptMail viewModel = new ReceiptMail
            {
                Purchases = GetPurchasesFromSession()
            };

            string userEmail = User.Identity.Name;
            var items = GetPurchasesFromSession();

            var html = this._viewRender.Render("Cart/Receipt", new ReceiptMail
            {
                UserName = userEmail,
                Purchases = items
            });

            _emailSender.SendEmailAsync(userEmail, "Receipt", html);

            return View(viewModel);
        }

        public IActionResult Confirm()
        {
            return View();
        }

        private IEnumerable<Purchase> GetPurchasesFromSession()
        {
            List<ShoppingProduct> products = HttpContext.Session.GetObject<List<ShoppingProduct>>(WebConstants.CartKey);
            if (products == null)
            {
                products = new List<ShoppingProduct>();
            }

            int[] productIds = products.Select(i => i.ProductId).ToArray();

            return _unitOfWork.PurchaseRepository.Get(c => productIds.Contains(c.Id), null, nameof(Category));
        }
    }
}
