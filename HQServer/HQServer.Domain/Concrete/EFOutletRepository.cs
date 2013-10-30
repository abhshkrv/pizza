using HQServer.Domain.Abstract;
using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace HQServer.Domain.Concrete
{
    public class EFOutletRepository : IOutletRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Outlet> Outlets
        {
            get { return context.Outlets; }
        }

        public void saveOutlet(Outlet outlet)
        {
            if (outlet.outletID == 0)
            {
                context.Outlets.Add(outlet);
                context.SaveChanges();
            }
            else
            {
                context.Entry(outlet).State = EntityState.Modified;
                context.SaveChanges();
            }

        }

        public void deleteOutlet(Outlet outlet)
        {
            context.Outlets.Remove(outlet);
            context.SaveChanges();
        }

        public void deleteTable()
        {


        }
    }
}
