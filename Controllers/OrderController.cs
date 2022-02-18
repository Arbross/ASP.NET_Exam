using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Exam_ASP_NET.Models;
using Exam_ASP_NET.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReplicaData.Repositories;

namespace Exam_ASP_NET.Controllers
{
    [Authorize(Roles = WebConstants.AdminRole)]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(DatabaseContext context)
        {
            _unitOfWork = new UnitOfWork(context);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetOrdersAPI()
        {
            return new JsonResult(new { data = _unitOfWork.PurchaseRepository.Get() });
        }
    }
}
