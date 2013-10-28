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
    public class EFOutletTransactionRepository : IOutletTransactionRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<OutletTransaction> OutletTransactions
        {
            get { return context.OutletTransactions; }
        }

        public void saveOutletTransaction(OutletTransaction outletTransaction)
        {
            if (context.Entry(outletTransaction).State == EntityState.Detached)
            {
                context.OutletTransactions.Attach(outletTransaction);
            }

            context.Entry(outletTransaction).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void deleteOutletTransaction(OutletTransaction outletTransaction)
        {
            context.OutletTransactions.Remove(outletTransaction);
            context.SaveChanges();
        }

        public void deleteTable()
        {


        }
    }
}
