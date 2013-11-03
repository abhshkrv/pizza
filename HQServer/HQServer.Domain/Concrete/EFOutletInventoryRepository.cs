﻿using HQServer.Domain.Abstract;
using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HQServer.Domain.Concrete
{
    public class EFOutletInventoryRepository : IOutletInventoryRepository    
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<OutletInventory> OutletInventories
        {
            get { return context.OutletInventories; }
        }

        public void quickSaveOutletInventory(OutletInventory outletInventory)
        {
            context.OutletInventories.Add(outletInventory);

        }

        public void saveContext()
        {
            context.SaveChanges();
        }

        public void deleteOutletInventory(OutletInventory outletInventory)
        {
            context.OutletInventories.Remove(outletInventory);
            context.SaveChanges();
        }

        public void deleteTable()
        {


        }
    }
}
