using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HQServer.Domain.Abstract
{
    public interface IOutletTransactionDetailRepository
    {
        IQueryable<OutletTransaction> OutletTransactionDetails { get; }
        void saveOutletTransactionDetail(OutletTransaction outletTransactionDetail);
        void deleteOutletTransactionDetail(OutletTransaction outletTransactionDetail);
    }
}
