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
        IBatchDispatchDetailRepository _batchDispatchRepo;
        IBatchDispatchRepository _batchDispatchDetailRepo;
        public BatchController(IBatchResponseRepository brepo, IBatchResponseDetailRepository bdrepo, IProductRepository prepo)
        {

            _productRepo = prepo;
            _batchResponseRepo = brepo;
            _batchResponseDetailRepo = bdrepo;
        }

        [HttpPost]
        public ContentResult Send(string shopID, string barcode, string qty, string requestID, string comment)
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
                batchResponse.comments = comment;
                batchResponse.outletID = Int32.Parse(shopID);
                batchResponse.timestamp = DateTime.Now;
                batchResponse.requestID = Int16.Parse(requestID);

                _batchResponseRepo.saveBatchResponse(batchResponse);
                batchResponseDetail.batchResponseID = batchResponse.batchResponseID;
                batchResponseDetail.barcode = Int32.Parse(barcode);
                batchResponseDetail.quantity = Int32.Parse(qty);
                _batchResponseDetailRepo.saveBatchResponseDetail(batchResponseDetail);
                return new ContentResult()
                {
                    Content = "{Status:Success}",
                    ContentType = "application/json",
                };
            }
        }

        public ActionResult viewRequests()
        {
            var req = _batchResponseRepo.BatchResponses;
            return View(req);
        }

        public ActionResult viewRequest(string id)
        {
            //BatchResponse br = _batchResponseRepo.BatchResponses.FirstOrDefault(b => b.requestID.ToString() == requestID & b.outletID.ToString() == outletID);
            var batchResponseDetails = _batchResponseDetailRepo.BatchResponseDetails.Where(b => b.batchResponseID.ToString() == id).ToList();

            return View(batchResponseDetails);
        }

        public ActionResult attend(string id)
        {
            BatchResponse br = _batchResponseRepo.BatchResponses.FirstOrDefault(b => b.batchResponseID.ToString() == id);
            br.status = Status.RESPONDED;
            _batchResponseRepo.saveBatchResponse(br);
            return View();
        }

        [HttpPost]
        public string acknowledge(string outletID, string requestID)
        {
            BatchResponse br = _batchResponseRepo.BatchResponses.FirstOrDefault(b => b.requestID.ToString() == requestID & b.outletID.ToString() == outletID);
            if (br == null)
                return "Fail";
            br.status = Status.ACKNOWLEDGED;
            _batchResponseRepo.saveBatchResponse(br);
            return "Success";
        }

        public ActionResult dispatchItems(string shopID)
        {
            //var 
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
