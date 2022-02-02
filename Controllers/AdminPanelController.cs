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
    public class AdminPanelController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _host;

        public AdminPanelController(DatabaseContext context, IWebHostEnvironment host)
        {
            _unitOfWork = new UnitOfWork(context);
            _host = host;
        }

        public IActionResult AdminPanel()
        {
            AdminPanelVM viewModel = new AdminPanelVM()
            {
                Search = new Search(),
                Purchases = _unitOfWork.PurchaseRepository.Get(includeProperties: nameof(Category)),
                Categories = _unitOfWork.GetCategories()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(WebConstants.AdminRole)]
        public IActionResult Search(AdminPanelVM model)
        {
            if (!ModelState.IsValid) return NotFound();

            if (model.Search.Text == null && model.Search.CategoryId <= 0)
            {
                var list = _unitOfWork.PurchaseRepository.Get(x => x.Id > 0, null, nameof(Category));

                if (list == null) return NotFound();
                model.Purchases = list;
            }
            else if (model.Search.Text != null && model.Search.CategoryId > 0)
            {   
                var list = _unitOfWork.PurchaseRepository.Get(x => (x.Name.Contains(model.Search.Text) || x.Name == model.Search.Text) && x.Category.Id == model.Search.CategoryId, null, nameof(Category)); ;

                if (list == null) return NotFound();
                model.Purchases = list;
            }
            else if (model.Search.CategoryId > 0)
            {
                var list = _unitOfWork.PurchaseRepository.Get(x => x.Category.Id == model.Search.CategoryId, null, nameof(Models.Purchase.Category));

                if (list == null) return NotFound();
                model.Purchases = list;
            }
            else if (model.Search.Text != null)
            {
                var list = _unitOfWork.PurchaseRepository.Get(x => x.Name.Contains(model.Search.Text) || x.Name == model.Search.Text, null, nameof(Models.Purchase.Category));

                if (list == null) return NotFound();
                model.Purchases = list;
            }

            AdminPanelVM viewModel = new AdminPanelVM()
            {
                Search = model.Search,
                Purchases = model.Purchases,
                Categories = _unitOfWork.GetCategories()
            };

            return View(nameof(AdminPanel), viewModel);
        }

        private string SaveCarImage(IFormFile img)
        {
            string root = _host.WebRootPath;
            string folder = root + WebConstants.ImagesPath;
            string name = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(img.FileName);

            string fullPath = Path.Combine(folder, name + extension);

            using (FileStream fs = new FileStream(fullPath, FileMode.Create))
            {
                img.CopyTo(fs);
            }

            return name + extension;
        }

        // GET
        [Authorize(WebConstants.AdminRole)]
        public IActionResult Upset(int? id)
        {
            AdminPanelVM viewModel = new AdminPanelVM()
            {
                Purchase = new Purchase(),
                Categories = _unitOfWork.GetCategories()
            };

            if (id == null)
            {
                return View(viewModel);
            }
            else
            {
                viewModel.Purchase = _unitOfWork.PurchaseRepository.GetById(id);
                if (viewModel.Purchase == null) return NotFound();

                return View(viewModel);
            }
        }

        // POST
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(WebConstants.AdminRole)]
        public IActionResult Upset(AdminPanelVM model)
        {
            if (!ModelState.IsValid) return NotFound();

            if (model.Purchase.Id == 0)
            {
                var files = HttpContext.Request.Form.Files;

                string fileName = SaveCarImage(files[0]);
                model.Purchase.Image = fileName;

                _unitOfWork.Add(model.Purchase);
                _unitOfWork.SaveChangesAsync();
            }
            else
            {
                var files = HttpContext.Request.Form.Files;
                //var oldCar = _context.Purchases.AsNoTracking().FirstOrDefault(c => c.Id == model.Purchase.Id);
                var oldCar = _unitOfWork.PurchaseRepository.GetById(model.Purchase.Id);

                if (files.Any())
                {
                    if (oldCar.Image != null)
                    {
                        string oldCarImagePath = _host.WebRootPath + Path.Combine(WebConstants.ImagesPath, oldCar.Image);

                        if (System.IO.File.Exists(oldCarImagePath))
                        {
                            System.IO.File.Delete(oldCarImagePath);
                        }
                    }

                    string fileName = SaveCarImage(files[0]);

                    model.Purchase.Image = fileName;
                }
                else
                {
                    model.Purchase.Image = oldCar.Image;
                }

                _unitOfWork.Update(model.Purchase);
                _unitOfWork.SaveChangesAsync();
            }


            return RedirectToAction(nameof(AdminPanel));
        }

        [Authorize(WebConstants.AdminRole)]
        public IActionResult Delete(int? id)
        {
            if (id == null || id <= 0) return NotFound();

            var remove = _unitOfWork.PurchaseRepository.GetById(id);

            if (remove == null) return NotFound();

            if (remove.Image != null)
            {
                string imagePath = _host.WebRootPath + Path.Combine(WebConstants.ImagesPath, remove.Image);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _unitOfWork.Remove(remove);
            _unitOfWork.SaveChangesAsync();

            return RedirectToAction(nameof(AdminPanel));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
