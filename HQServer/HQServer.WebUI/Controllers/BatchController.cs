using HQServer.Domain.Abstract;
using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HQServer.WebUI.Controllers
{
    public class BatchController : Controller
    {
        IBatchResponseRepository _batchResponseRepo;
        IProductRepository _productRepo;
        IBatchResponseDetailRepository _batchResponseDetailRepo;
        public BatchController(IBatchResponseRepository brepo, IBatchResponseDetailRepository bdrepo, IProductRepository prepo)
        {

            _productRepo = prepo;
            _batchResponseRepo = brepo;
            _batchResponseDetailRepo = bdrepo;
        }

        [HttpPost]
        public ContentResult Send(string shopID, string barcode, string qty, string requestID)
        {
            Product product = _productRepo.Products.FirstOrDefault(p => p.barcode == barcode);
            if (product == null)
            {
                ViewBag["Result"] = "Error";
                return new ContentResult()
                {
                    Content = "{Status:Fail}",
                    ContentType = "application/json",
                };
            }
            else
            {
                BatchResponse batchResponse = new BatchResponse();
                BatchResponseDetail batchResponseDetail = new BatchResponseDetail();
                batchResponse.status = Status.NOT_RESPONDED;
                batchResponse.timestamp = DateTime.Now;
                batchResponse.requestID = Int16.Parse(requestID);

                _batchResponseRepo.saveBatchResponse(batchResponse);
                batchResponseDetail.requestID = batchResponse.requestID;
                batchResponseDetail.barcode = Int32.Parse(barcode);
                batchResponseDetail.quantity = Int32.Parse(qty);

                return new ContentResult()
                {
                    Content = "{Status:Success}",
                    ContentType = "application/json",
                };
            }
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
