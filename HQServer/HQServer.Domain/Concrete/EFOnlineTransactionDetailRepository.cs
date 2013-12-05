using HQServer.Domain.Abstract;
using HQServer.Domain.Concrete;
using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopping.Domain.Concrete
{
    public class EFOnlineTransactionDetailRepository : IOnlineTransactionDetailRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<OnlineTransactionDetail> TransactionDetails
        {
            get { return context.TransactionDetails; }
        }

        public void saveTransactionDetail(OnlineTransactionDetail transactionDetail)
        {
            if (context.Entry(transactionDetail).State == EntityState.Detached)
            {
                context.TransactionDetails.Add(transactionDetail);
            }

            // context.Entry(Transaction).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void quickSaveTransactionDetail(OnlineTransactionDetail transactionDetail)
        {
            context.TransactionDetails.Add(transactionDetail);
        }

        public void saveContext()
        {
            context.SaveChanges();
        }

        public void deleteTransactionDetail(OnlineTransactionDetail TransactionDetail)
        {
            context.TransactionDetails.Remove(TransactionDetail);
            context.SaveChanges();
        }

        public void deleteTable()
        {


        }
    }
}