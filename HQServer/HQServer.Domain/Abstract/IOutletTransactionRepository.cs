using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HQServer.Domain.Abstract
{
    public interface IOutletTransactionRepository
    {
        IQueryable<OutletTransaction> OutletTransactions { get; }
        void saveOutletTransaction(OutletTransaction outletTransaction);
        void deleteOutletTransaction(OutletTransaction outletTransaction);
    }
}
