using HQServer.Domain.Abstract;
using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HQServer.WebUI.Controllers
{
    public class ProductController : Controller
    {

        IProductRepository _productRepo;
        ICategoryRepository _categoryRepo;
        IManufacturerRepository _manufacturerRepo;
        int categoryCount = 0;
        int manufacturerCount = 0;

        public ProductController(IProductRepository productRepo, ICategoryRepository categoryRepo, IManufacturerRepository manufacturerRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _manufacturerRepo = manufacturerRepo;
        }
        //
        // GET: /Product/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Setup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Setup(HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);
            if (file.ContentLength > 0)
            {
                
                var path = Path.Combine(Server.MapPath("~/Content/ProductInventory"), fileName);
                file.SaveAs(path);
                emptydatabase();
            }
            else
            {
                return View();
            }

            if (parseFile(fileName))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        private void emptydatabase()
        {
           deleteProductTable();
           deleteCategoryTable();
           deleteManufactureTable();
        }
        
        public void deleteProductTable()
        {
            _productRepo.deleteTable();
        }

        public void deleteManufactureTable()
        {
            _manufacturerRepo.deleteTable();
        }

        public void deleteCategoryTable()
        {
            _categoryRepo.deleteTable();
        }

        private bool parseFile(string fileName)
        {
            emptydatabase();   
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath(@"~/Content/ProductInventory/"+fileName));
            List<string> inputList = lines.Cast<string>().ToList();
            var manufacturersList = new Dictionary<string, int>();
            var categoriesList = new Dictionary<string, int>();
            foreach (string i in inputList)
            {
                string[] tokens = i.Split(':');
                Product product = new Product();
                product.productName = tokens[0];
                string categoryName = tokens[1];
                Category category = null;
                int categoryID;
                if (categoriesList.ContainsKey(categoryName) == false)
                {
                    category = new Category();
                    categoryID = categoriesList.Count() + 1;
                    category.categoryName = categoryName;
                    categoriesList.Add(categoryName, categoryID);
                    _categoryRepo.quickSaveCategory(category);

                }
                product.categoryID = categoriesList[categoryName];

                string manufacturerName = tokens[2];
                int manufacturerID;
                Manufacturer manufacturer=null;
                if(manufacturersList.ContainsKey(manufacturerName)==false)       
                {
                   manufacturer = new Manufacturer();
                   manufacturerID = manufacturersList.Count() + 1;
                   manufacturer.manufacturerName = manufacturerName;
                   manufacturersList.Add(manufacturerName,manufacturerID);
                   _manufacturerRepo.quickSaveManufacturer(manufacturer);

                }
                product.manufacturerID = manufacturersList[manufacturerName];                  
                product.barcode = tokens[3];
                product.costPrice = float.Parse(tokens[4]);
                product.currentStock = int.Parse(tokens[5]);
                product.minimumStock = int.Parse(tokens[6]);
                product.bundleUnit = int.Parse(tokens[7]);

                _productRepo.quickSaveProduct(product);
            }
            
            _manufacturerRepo.saveContext();
            _categoryRepo.saveContext();
            _productRepo.saveContext();

            return true;
        }

    }
}
