using HQServer.Domain.Abstract;
using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            if (context.Entry(batchResponseDetail).State == EntityState.Detached)
            {
                context.BatchResponseDetails.Add(batchResponseDetail);
            }

            // context.Entry(BatchResponse).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void quickSaveBatchResponseDetail(BatchResponseDetail batchResponseDetail)
        {
            context.BatchResponseDetails.Add(batchResponseDetail);
        }

        public void saveContext()
        {
            context.SaveChanges();
        }

        public void deleteBatchResponseDetail(BatchResponseDetail batchResponseDetail)
        {
            context.BatchResponseDetails.Remove(batchResponseDetail);
            context.SaveChanges();
        }

        
    }
}
