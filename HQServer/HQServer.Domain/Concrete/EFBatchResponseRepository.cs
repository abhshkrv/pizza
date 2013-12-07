using HQServer.Domain.Abstract;
using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace HQServer.Domain.Concrete
{
    public class EFBatchResponseRepository : IBatchResponseRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<BatchResponse> BatchResponses
        {
            get { return context.BatchResponses; }
        }

        public void saveBatchResponse(BatchResponse batchResponse)
        {
            if (batchResponse.batchResponseID == 0)
            {
                context.BatchResponses.Add(batchResponse);
                context.SaveChanges();
            }
            else 
            {
                context.Entry(batchResponse).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void deleteBatchResponse(BatchResponse batchResponse)
        {
            context.BatchResponses.Remove(batchResponse);
            context.SaveChanges();
        }

        public void deleteTable()
        {


        }
    }
}
