﻿using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HQServer.Domain.Abstract
{
    public interface IOutletInventoryRepository
    {
        IQueryable<OutletInventory> OutletInventories { get; }
        void saveOutletInventory(OutletInventory outletInventory);
        void deleteOutletInventory(OutletInventory outletInventory);
    }
}
