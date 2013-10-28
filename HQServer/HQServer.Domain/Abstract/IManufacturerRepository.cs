using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HQServer.Domain.Abstract
{
    public interface IManufacturerRepository
    {
        IQueryable<Manufacturer> Manufacturers { get; }
        void saveManufacturer(Manufacturer manufacturer);
        void quickSaveManufacturer(Manufacturer manufacturer);
        void deleteManufacturer(Manufacturer manufacturer);
        void saveContext();
        void deleteTable();
    }
}
