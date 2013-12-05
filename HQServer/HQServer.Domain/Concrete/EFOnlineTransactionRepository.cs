using HQServer.Domain.Abstract;
using HQServer.Domain.Concrete;
using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace OnlineShopping.Domain.Concrete
{
    public class EFOnlineTransactionRepository : IOnlineTransactionRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<OnlineTransaction> Transactions
        {
            get { return context.Transactions; }
        }

        public void saveTransaction(OnlineTransaction Transaction)
        {
            if (context.Entry(Transaction).State == EntityState.Detached)
            {
                context.Transactions.Add(Transaction);
            }

            // context.Entry(Transaction).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void quickSaveTransaction(OnlineTransaction Transaction)
        {
            context.Transactions.Add(Transaction);
        }

        public void saveContext()
        {
            context.SaveChanges();
        }

        public void deleteTransaction(OnlineTransaction Transaction)
        {
            context.Transactions.Remove(Transaction);
            context.SaveChanges();
        }

        public void deleteTable()
        {


        }
    }
}