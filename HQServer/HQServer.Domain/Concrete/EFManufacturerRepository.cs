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
    public class EFManufacturerRepository : IManufacturerRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Manufacturer> Manufacturers
        {
            get { return context.Manufacturers; }
        }

        public void quickSaveManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer.manufacturerID == 0)
            {
                context.Manufacturers.Add(manufacturer);
            }
            else
            {
                context.Entry(manufacturer).State = EntityState.Modified;

            }
        }

        public void saveManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer.manufacturerID == 0)
            {
                context.Manufacturers.Add(manufacturer);
                context.SaveChanges();
            }
            else
            {
                context.Entry(manufacturer).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void saveContext()
        {
            context.SaveChanges();
        }

        public void deleteManufacturer(Manufacturer manufacturer)
        {
            context.Manufacturers.Remove(manufacturer);
            context.SaveChanges();
        }

        public void deleteTable()
        {
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE MANUFACTURERS");
        
        }
    }
}