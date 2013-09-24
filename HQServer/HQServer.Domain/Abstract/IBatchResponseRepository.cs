using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HQServer.Domain.Abstract
{
    public interface IBatchResponseRepository
    {
        IQueryable<BatchResponse> BatchResponses { get; }
        void saveBatchResponse(BatchResponse batchResponse);
        void deleteBatchResponse(BatchResponse batchResponse);
    }
}
