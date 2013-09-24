using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HQServer.Domain.Abstract
{
    public interface IOutletRepository
    {
        IQueryable<Outlet> Outlets { get; }
        void saveOutlet(Outlet outlet);
        void deleteOutlet(Outlet outlet);
    }
}
