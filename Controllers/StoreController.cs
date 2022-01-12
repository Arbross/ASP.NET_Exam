﻿using Exam_ASP_NET.Models;
using Exam_ASP_NET.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_ASP_NET
{
    public class StoreController : Controller
    {
        private DatabaseContext _context;
        private IWebHostEnvironment _host;

        public StoreController(DatabaseContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }

        public IActionResult Store()
        {
            return View(_context.Purchases);
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
        public IActionResult Upset(int? id)
        {
            ViewModel viewModel = new ViewModel()
            {
                Purchase = new Purchase(),
            };

            if (id == null)
            {
                return View(viewModel);
            }
            else
            {
                viewModel.Purchase = _context.Purchases.Find(id);
                if (viewModel.Purchase == null) return NotFound();

                return View(viewModel);
            }
        }

        // POST
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Upset(ViewModel model)
        {
            if (!ModelState.IsValid) return NotFound();

            if (model.Purchase.Id == 0)
            {
                var files = HttpContext.Request.Form.Files;

                string fileName = SaveCarImage(files[0]);
                model.Purchase.Image = fileName;

                _context.Purchases.Add(model.Purchase);
                _context.SaveChanges();
            }
            else
            {
                var files = HttpContext.Request.Form.Files;
                var oldCar = _context.Purchases.AsNoTracking().FirstOrDefault(c => c.Id == model.Purchase.Id);

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

                _context.Purchases.Update(model.Purchase);
                _context.SaveChanges();
            }


            return RedirectToAction(nameof(Store));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id <= 0) return NotFound();

            var remove = _context.Purchases.Find(id);

            if (remove == null) return NotFound();

            if (remove.Image != null)
            {
                string imagePath = _host.WebRootPath + Path.Combine(WebConstants.ImagesPath, remove.Image);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Purchases.Remove(remove);
            _context.SaveChanges();

            return RedirectToAction(nameof(Store));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}