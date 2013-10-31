using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HQServer.Domain.Entities;

namespace HQServer.WebUI.Models
{
    public class TransactionListViewModel
    {
        public IEnumerable<OutletTransaction> Transactions { get; set; }
        public IEnumerable<OutletTransactionDetail> TransactionDetail { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}