using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HQServer.Domain.Abstract
{
    public interface IBatchResponseDetailRepository
    {
        IQueryable<BatchResponseDetail> BatchResponseDetails { get; }
        void saveBatchResponseDetail(BatchResponseDetail batchResponseDetail);
        void deleteBatchResponseDetail(BatchResponseDetail batchResponseDetail);
    }
}
