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
        private IProductRepository _productRepo;
        public AdminController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }
        public int PageSize = 20;
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
            return View("Edit", new Product());
        }
        public ViewResult Details(int productId)
        {
            Product product = _productRepo.Products.FirstOrDefault(p => p.productID == productId);
            return View(product);
        }

        [HttpPost]
        public ActionResult Delete(int productId)
        {
            Product prod = _productRepo.Products.FirstOrDefault(p => p.productID == productId);
            if (prod != null)
            {
                _productRepo.deleteProduct(prod);
                TempData["message"] = string.Format("{0} was deleted", prod.productName);
            }
            return RedirectToAction("Index");
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