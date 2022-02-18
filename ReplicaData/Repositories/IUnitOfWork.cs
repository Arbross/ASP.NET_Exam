using Exam_ASP_NET.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplicaData.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Repository<Purchase> PurchaseRepository { get; }
        Repository<Category> CategoryRepository { get; }
        Repository<User> UserRepository { get; }
        void SaveChanges();
        void SaveChangesAsync();
        void Remove(Purchase purchase);
        void Remove(Category category);
        void Update(Purchase purchase);
        void Update(Category category);
        void Add(Category category);
        void Add(Purchase purchase);
        void Load(Purchase purchase);
        IEnumerable<SelectListItem> GetCategories();
    }
}
