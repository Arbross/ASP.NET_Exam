using Exam_ASP_NET;
using Exam_ASP_NET.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplicaData.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private DatabaseContext context;

        private Repository<Purchase> purchaseRepository;
        private Repository<Category> categoryRepository;
        private Repository<User> userRepository;

        public UnitOfWork(DatabaseContext context)
        {
            this.context = context;

            purchaseRepository = new Repository<Purchase>(context);
            categoryRepository = new Repository<Category>(context);
            userRepository = new Repository<User>(context);
        }

        public Repository<Purchase> PurchaseRepository => purchaseRepository;
        public Repository<Category> CategoryRepository => categoryRepository;
        public Repository<User> UserRepository => userRepository;

        public virtual void SaveChangesAsync() => context.SaveChangesAsync();
        public virtual void SaveChanges() => context.SaveChanges();

        public virtual void Remove(Purchase purchase) => context.Purchases.Remove(purchase);
        public virtual void Remove(Category category) => context.Categories.Remove(category);

        public virtual void Update(Purchase purchase) => context.Purchases.Update(purchase);
        public virtual void Update(Category category) => context.Categories.Update(category);

        public virtual void Add(Purchase purchase) => context.Purchases.Add(purchase);
        public virtual void Add(Category category) => context.Categories.Add(category);

        public virtual void Load(Purchase purchase) => context.Entry(purchase).Reference(nameof(Purchase.Category)).Load();

        public virtual IEnumerable<SelectListItem> GetCategories()
        {
            return context.Categories.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
