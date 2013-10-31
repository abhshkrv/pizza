using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HQServer.Domain.Abstract
{
    public interface IOutletTransactionDetailRepository
    {
        IQueryable<OutletTransactionDetail> OutletTransactionDetails { get; }
        void saveOutletTransactionDetail(OutletTransactionDetail outletTransactionDetail);
        void deleteOutletTransactionDetail(OutletTransactionDetail outletTransactionDetail);
        void quickSaveOutletTransactionDetail(OutletTransactionDetail outletTransactionDetail);
        void saveContext();
    }
}
