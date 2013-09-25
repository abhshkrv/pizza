using HQServer.Domain.Abstract;
using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
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
