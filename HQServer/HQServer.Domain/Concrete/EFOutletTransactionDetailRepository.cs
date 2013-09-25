using HQServer.Domain.Abstract;
using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HQServer.Domain.Concrete
{
    public class EFOutletTransactionDetailRepository : IOutletTransactionDetailRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<OutletTransactionDetail> OutletTransactionDetails
        {
            get { return context.OutletTransactionDetails; }
        }

        public void saveOutletTransactionDetail(OutletTransactionDetail outletTransactionDetail)
        {

        }

        public void deleteOutletTransactionDetail(OutletTransactionDetail outletTransactionDetail)
        {
            context.OutletTransactionDetails.Remove(outletTransactionDetail);
            context.SaveChanges();
        }

        public void deleteTable()
        {


        }
    }
}
