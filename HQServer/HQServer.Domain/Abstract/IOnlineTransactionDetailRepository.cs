using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HQServer.Domain.Abstract
{
    public interface IOnlineTransactionDetailRepository
    {
        IQueryable<OnlineTransactionDetail> TransactionDetails { get; }
        void saveTransactionDetail(OnlineTransactionDetail transactionDetail);
        void quickSaveTransactionDetail(OnlineTransactionDetail transactionDetail);
        void saveContext();
        void deleteTransactionDetail(OnlineTransactionDetail transactionDetail);
    }
}