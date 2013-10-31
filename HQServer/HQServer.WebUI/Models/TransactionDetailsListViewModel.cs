using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HQServer.WebUI.Models
{
    public class TransactionDetailsListViewModel
    {
        public OutletTransaction transaction { get; set; }
        public IEnumerable<OutletTransactionDetail> TransactionDetail { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}