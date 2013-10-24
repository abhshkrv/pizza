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
            /*var products = _productRepo.Products.Where(p=>true);

            foreach (Product product in products.ToList())
            {
                _productRepo.deleteProduct(product);
            } */
            _productRepo.deleteTable();
        }

        public void deleteManufactureTable()
        {
           /* var manufacturers = _manufacturerRepo.Manufacturers.Where(m=> true);

            foreach (Manufacturer manufacturer in manufacturers.ToList())
            {
                _manufacturerRepo.deleteManufacturer(manufacturer);
            } */
            _manufacturerRepo.deleteTable();
        }

        public void deleteCategoryTable()
        {
           /* var categories = _categoryRepo.Categories.Where(c=>true);

            foreach (Category c in categories.ToList())
            {
                _categoryRepo.deleteCategory(c);
            }*/
            _categoryRepo.deleteTable();
        }

        private bool parseFile(string fileName)
        {
            emptydatabase();   
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath(@"~/Content/ProductInventory/"+fileName));
            List<string> inputList = lines.Cast<string>().ToList();
            foreach (string i in inputList)
            {
                string[] tokens = i.Split(':');
                Product product = new Product();
                product.productName = tokens[0];
                Category category = new Category();
                category.categoryName = tokens[1];
                product.categoryID = getCategoryID(category);
                Manufacturer manufacturer = new Manufacturer();
                manufacturer.manufacturerName = tokens[2];
                product.manufacturerID = getManufacturerID(manufacturer);
                product.barcode = tokens[3];
                product.costPrice = float.Parse(tokens[4]);
                product.currentStock = int.Parse(tokens[5]);
                product.minimumStock = int.Parse(tokens[6]);
                product.bundleUnit = int.Parse(tokens[7]);

                _productRepo.saveProduct(product);
            }
            return true;
        }

        private int getCategoryID(Category category)
        {
            //Category result = _categoryRepo.Categories.First(c => c.categoryName == category.categoryName);

            //if (result != null)
            {
            //    return result.categoryID;
            }
           // else
            {
                _categoryRepo.saveCategory(category);
                return category.categoryID;
            }
        }

        private int getManufacturerID(Manufacturer manufacturer)
        {
            //Manufacturer result = _manufacturerRepo.Manufacturers.First(m => m.manufacturerName == manufacturer.manufacturerName);

//            if (result != null)
 //           {
     //           return result.manufacturerID;
   //         }
       //     else
            {
                _manufacturerRepo.saveManufacturer(manufacturer);
                return manufacturer.manufacturerID;
            }
        }
    }
}
