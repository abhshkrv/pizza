using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HQServer.Domain.Abstract
{
    public interface IBatchDispatchRepository
    {
        IQueryable<BatchDispatch> BatchDispatches { get; }
        void saveBatchDispatch(BatchDispatch batchDispatch);
        void deleteBatchDispatch(BatchDispatch batchDispatch);
    }
}
