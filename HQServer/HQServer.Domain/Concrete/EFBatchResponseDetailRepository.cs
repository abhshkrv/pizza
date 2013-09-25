using HQServer.Domain.Abstract;
using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HQServer.Domain.Concrete
{
    public class EFBatchResponseDetailRepository : IBatchResponseDetailRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<BatchResponseDetail> BatchResponseDetails
        {
            get { return context.BatchResponseDetails; }
        }

        public void saveBatchResponseDetail(BatchResponseDetail batchResponseDetail)
        {
                       
        }

        public void deleteBatchResponseDetail(BatchResponseDetail batchResponseDetail)
        {
            context.BatchResponseDetails.Remove(batchResponseDetail);
            context.SaveChanges();
        }

        public void deleteTable()
        {
            

        }
    }
}
