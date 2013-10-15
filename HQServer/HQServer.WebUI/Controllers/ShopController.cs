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

        public ShopController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
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
            for (int i = 0; i < 100; i++)
                result.Add(i.ToString(), _productRepo.Products);
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
