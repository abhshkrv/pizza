using System.Web.Mvc;
using HQServer.Domain.Abstract;
using HQServer.Domain.Entities;
using System.Linq;
using HQServer.WebUI.Models;
using System;
namespace HQServer.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        IProductRepository _productRepo;
        ICategoryRepository _categoryRepo;
        IManufacturerRepository _manufacturerRepo;
        IOutletInventoryRepository _outletInventoryRepo;

        public AdminController(IProductRepository productRepo, ICategoryRepository categoryRepo, IManufacturerRepository manufacturerRepo,
                            IOutletInventoryRepository outletInventoryRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _manufacturerRepo = manufacturerRepo;
            _outletInventoryRepo = outletInventoryRepo;
     
        }
        public int PageSize = 200;
        public ViewResult Index(int page = 1)
        {
            ProductsListViewModel viewModel = new ProductsListViewModel
            {
                Products = _productRepo.Products
                .OrderBy(p => p.productID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _productRepo.Products.Count()
                }
            };

            return View(viewModel);
        }
        public ViewResult Edit(int productId)
        {
            Product product = _productRepo.Products.FirstOrDefault(p => p.productID == productId);
            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepo.saveProduct(product);
                TempData["message"] = string.Format("{0} has been saved", product.productName);
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(product);
            }
        }
      
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(string name = null, string barcode = null, string manufacturerName = null, 
                                   string categoryName = null, string costPrice = null, string maximumPrice=null,
                                   string currentStock = null, string minimumStock = null, string bundleUnit = null,
                                   string discountPercentage= null)
        {
            Product product = new Product();

            if (ModelState.IsValid)
            {
                
                product.productName = name;
                Category category = _categoryRepo.Categories.FirstOrDefault(c => c.categoryName == categoryName);
                if (category == null)
                    category = new Category();
                category.categoryName = categoryName;                
                product.categoryID = getCategoryID(category);
                Manufacturer manufacturer = _manufacturerRepo.Manufacturers.FirstOrDefault(m => m.manufacturerName == manufacturerName);
                if (manufacturer == null)
                    manufacturer = new Manufacturer();
                manufacturer.manufacturerName = manufacturerName;
                product.manufacturerID = getManufacturerID(manufacturer);
                product.barcode = barcode;
                product.costPrice = float.Parse(costPrice);
                product.currentStock = int.Parse(currentStock);
                product.minimumStock = int.Parse(minimumStock);
                product.bundleUnit = int.Parse(bundleUnit);
                product.maxPrice = float.Parse(maximumPrice);
                product.discountPercentage = float.Parse(discountPercentage);
                 _productRepo.saveProduct(product);
                 _productRepo.saveContext();
                 _manufacturerRepo.saveContext();
                 _categoryRepo.saveContext();
                TempData["message"] = string.Format("{0} has been saved", product.productName);
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(product);
            }
            

        }

        private int getCategoryID(Category category)
        {
            _categoryRepo.saveCategory(category);
            return category.categoryID;
        }

        private int getManufacturerID(Manufacturer manufacturer)
        {
            _manufacturerRepo.saveManufacturer(manufacturer);
            return manufacturer.manufacturerID;
        }
        public ViewResult Details(int productId)
        {

            ProductsDetailsViewModel viewModel = new ProductsDetailsViewModel();
            viewModel.product = _productRepo.Products.FirstOrDefault(p => p.productID == productId);
            viewModel.manufacturer = _manufacturerRepo.Manufacturers.FirstOrDefault(m =>m.manufacturerID == viewModel.product.manufacturerID);
            viewModel.category = _categoryRepo.Categories.FirstOrDefault(c =>c.categoryID == viewModel.product.categoryID );

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Delete(int productId)
        {

            Product prod = _productRepo.Products.FirstOrDefault(p => p.productID == productId);
            int bc = Int32.Parse(prod.barcode);
            OutletInventory outletInventory = _outletInventoryRepo.OutletInventories.FirstOrDefault(o => o.barcode == bc && o.currentStock != 0);
            if (outletInventory == null)
            {
                if (prod != null)
                {
                    _productRepo.deleteProduct(prod);
                    TempData["message"] = string.Format("{0} was deleted", prod.productName);
                }

            }
            else TempData["message"] = "Item in production";
            return RedirectToAction("Index");
        }


/* CRUD for manufacturers */

        public ViewResult AddManufacturer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddManufacturer(string manufacturerName = null)
        {
            Manufacturer manufacturer = new Manufacturer();

            if (ModelState.IsValid)
            {                
                manufacturer.manufacturerName = manufacturerName;
                _manufacturerRepo.saveManufacturer(manufacturer);
                TempData["message"] = string.Format("{0} has been saved", manufacturer.manufacturerName);
                return RedirectToAction("ManufacturersList");
            }
            else
            {
                // there is something wrong with the data values
                return View(manufacturer);
            }


        }
        
        public ViewResult ManufacturersList(int page = 1)
        {
            ManufacturersListViewModel viewModel = new ManufacturersListViewModel
            {
                Manufacturers = _manufacturerRepo.Manufacturers
                .OrderBy(m => m.manufacturerID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _manufacturerRepo.Manufacturers.Count()
                }
            };

            return View(viewModel);
        }

        public ViewResult EditManufacturer(int manufacturerId)
        {
            Manufacturer manufacturer = _manufacturerRepo.Manufacturers.FirstOrDefault(m=>m.manufacturerID == manufacturerId);
            return View(manufacturer);
        }
        [HttpPost]
        public ActionResult EditManufacturer(Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                _manufacturerRepo.saveManufacturer(manufacturer);
                TempData["message"] = string.Format("{0} has been saved", manufacturer.manufacturerName);
                return RedirectToAction("ManufacturersList");
            }
            else
            {
                // there is something wrong with the data values
                return View(manufacturer);
            }
        }
        [HttpPost]
        public ActionResult DeleteManufacturer(int manufacturerId)
        {

            Manufacturer manufacturer = _manufacturerRepo.Manufacturers.FirstOrDefault(m => m.manufacturerID == manufacturerId);
//OutletInventory outletInventory = _outletInventoryRepo.OutletInventories.FirstOrDefault(o => o.barcode ==bc && o.currentStock!=0);
           // if (outletInventory == null)
            //{
                if (manufacturer != null)
                {
                    _manufacturerRepo.deleteManufacturer(manufacturer);
                    TempData["message"] = string.Format("{0} was deleted", manufacturer.manufacturerName);
                }
                              
           // }
           // else TempData["message"] = "Item in production";
            return RedirectToAction("ManufacturersList");
        }

        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ProcessSearch(string name = null, string barcode = null, string manufacturer = null, string category = null)
        {
            ProductsListViewModel viewModel = new ProductsListViewModel();
            String noResults = "No product found with ";

            if (barcode != "" && barcode != null)
            {
                viewModel.Products = _productRepo.Products.Where(p => p.barcode == barcode);
                noResults += "barcode = " + barcode;
            }
            else if (name != "" && name != null)
            {
                viewModel.Products = _productRepo.Products.Where(p => p.productName.Contains(name));
                noResults += "Name = " + name;
            }

            if (viewModel.Products.Count() != 0)
                return View("SearchResults", viewModel);
            else
            {
                TempData["results"] = noResults;
                return View("Search");
            }

        }

    }
}