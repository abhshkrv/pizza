using HQServer.Domain.Abstract;
using HQServer.Domain.Entities;
using HQServer.WebUI.Models;
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
        IOutletRepository _outletRepo;

        public ShopController(IProductRepository productRepo, ICategoryRepository categoryRepo, IManufacturerRepository manufacturerRepo, IOutletRepository outletRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _manufacturerRepo = manufacturerRepo;
            _outletRepo = outletRepo;
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

        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(string owner = null, string address = null)
        {
            Outlet outlet = new Outlet();

            if (ModelState.IsValid)
            {
                outlet.owner = owner;
                outlet.address = address;
                _outletRepo.saveOutlet(outlet);
                TempData["message"] = string.Format("{0} has been saved", outlet.outletID);
                return RedirectToAction("../Home/Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(outlet);
            }


        }

        public int PageSize = 200;
        public ViewResult List(int page = 1)
        {
            OutletsListViewModel viewModel = new OutletsListViewModel
            {
                Outlets = _outletRepo.Outlets
                .OrderBy(o => o.outletID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _outletRepo.Outlets.Count()
                }
            };

            return View(viewModel);
        }

        public ViewResult Edit(int outletId)
        {
            Outlet outlet = _outletRepo.Outlets.FirstOrDefault(o => o.outletID == outletId);
            return View(outlet);
        }
        [HttpPost]
        public ActionResult Edit(Outlet outlet)
        {
            if (ModelState.IsValid)
            {
                _outletRepo.saveOutlet(outlet);
                TempData["message"] = string.Format("{0} has been saved", outlet.owner);
                return RedirectToAction("List");
            }
            else
            {
                // there is something wrong with the data values
                return View(outlet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int outletId)
        {
            Outlet outlet = _outletRepo.Outlets.FirstOrDefault(o => o.outletID == outletId);
            if (outlet != null)
            {
                _outletRepo.deleteOutlet(outlet);
                TempData["message"] = string.Format("{0} was deleted", outlet.owner);
            }
            return RedirectToAction("List");
        }


    }
}
