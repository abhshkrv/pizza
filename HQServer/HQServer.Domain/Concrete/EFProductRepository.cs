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
    public class EFProductRepository : IProductRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Product> Products
        {
            get { return context.Products; }
        }

        public void saveProduct(Product product)
        {
            if (product.productID == 0)
            {
                context.Products.Add(product);
                context.SaveChanges();
            }
            else
            {
                context.Entry(product).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void deleteProduct(Product product)
        {
            context.Products.Remove(product);
            context.SaveChanges();
        }

        public void deleteTable()
        {
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE PRODUCTS");

        }
    }
}