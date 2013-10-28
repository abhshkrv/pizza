using HQServer.Domain.Abstract;
using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HQServer.WebUI.Controllers
{
    public class ShopController : Controller
    {
        IProductRepository _productRepo;
        ICategoryRepository _categoryRepo;
        IManufacturerRepository _manufacturerRepo;

        public ShopController(IProductRepository productRepo, ICategoryRepository categoryRepo, IManufacturerRepository manufacturerRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _manufacturerRepo = manufacturerRepo;
        }

        //
        // GET: /Shop/

        public ActionResult Index()
        {
            return View();
        }

        public ContentResult getFullInventoryList()
        {

            var result = new Dictionary<string, object>();
            result.Add("status", "success");
            result.Add("Products", _productRepo.Products);

            //result.Add(i.ToString(), _productRepo.Products);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(result),
                ContentType = "application/json",
            };
        }

        public ContentResult getFullCategoriesList()
        {

            var result = new Dictionary<string, object>();
            result.Add("status", "success");
            result.Add("Categories", _categoryRepo.Categories);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(result),
                ContentType = "application/json",
            };
        }

        public ContentResult getFullManufacturersList()
        {

            var result = new Dictionary<string, object>();
            result.Add("status", "success");
            result.Add("Manufacturers", _manufacturerRepo.Manufacturers);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(result),
                ContentType = "application/json",
            };
        }



    }
}
