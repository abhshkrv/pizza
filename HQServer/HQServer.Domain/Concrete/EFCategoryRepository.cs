using HQServer.Domain.Abstract;
using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace HQServer.Domain.Concrete
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Category> Categories
        {
            get { return context.Categories; }
        }

        public void saveCategory(Category category)
        {
            if (category.categoryID == 0)
            {
                context.Categories.Add(category);
                context.SaveChanges();
            }
            else
            {
                context.Entry(category).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void deleteCategory(Category category)
        {
            context.Categories.Remove(category);
            context.SaveChanges();
        }

        public void deleteTable()
        {
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE CATEGORIES");
           
        }
    }
}