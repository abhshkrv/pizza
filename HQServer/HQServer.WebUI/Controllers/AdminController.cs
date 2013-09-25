using System.Web.Mvc;
using HQServer.Domain.Abstract;
using HQServer.Domain.Entities;
using System.Linq;
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
        public ViewResult Index()
        {
            return View(_productRepo.Products);
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
    }
}