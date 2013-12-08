using HQServer.Domain.Abstract;
using HQServer.Domain.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;



namespace HQServer.WebUI.Controllers
{
    public class BatchController : Controller
    {
        IBatchResponseRepository _batchResponseRepo;
        IProductRepository _productRepo;
        IBatchResponseDetailRepository _batchResponseDetailRepo;
        IBatchDispatchDetailRepository _batchDispatchDetailRepo;
        IBatchDispatchRepository _batchDispatchRepo;
        IOutletInventoryRepository _outletInventoryRepo;

        public BatchController(IBatchResponseRepository brepo, IBatchResponseDetailRepository bdrepo, IProductRepository prepo, IBatchDispatchRepository bdispatchrepo, IBatchDispatchDetailRepository bddrepo, IOutletInventoryRepository outletInventoryRepo)
        {

            _productRepo = prepo;
            _batchResponseRepo = brepo;
            _batchResponseDetailRepo = bdrepo;
            _batchDispatchDetailRepo = bddrepo;
            _batchDispatchRepo = bdispatchrepo;
            _outletInventoryRepo = outletInventoryRepo;

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

        public ContentResult dispatchItems(string shopID)
        {
            int id = Int16.Parse(shopID);
            var inventoryList = _outletInventoryRepo.OutletInventories.Where(o => o.outletID == id).ToArray();

            BatchDispatch batch = new BatchDispatch();
            batch.outletID = Int32.Parse(shopID);
            batch.status = DispatchStatus.NOT_RESPONDED;
            batch.timestamp = DateTime.Now;

            _batchDispatchRepo.saveBatchDispatch(batch);

            var qtyList = new Dictionary<string, int>();

            foreach (var item in inventoryList)
            {
                BatchDispatchDetail detail = new BatchDispatchDetail();

                detail.batchDispatchID = batch.batchDispatchID;
                detail.barcode = Int32.Parse(item.barcode);
                detail.quantity = toSendQty(item.currentStock, item.afterUpdateStock);
                if (detail.quantity != 0)
                {
                    qtyList.Add(detail.barcode.ToString(), detail.quantity);
                    _batchDispatchDetailRepo.quickSaveBatchDispatchDetail(detail);
                }
            }

            _batchDispatchDetailRepo.saveContext();

            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            createBlob(serializer.Serialize(qtyList));
            return new ContentResult()
            {
                Content = serializer.Serialize(qtyList),
                ContentType = "application/json",
            };
        }

        public void createBlob(string content)
        {
            // Account name and key.  Modify for your account.
            string accountName = "newdata";
            string accountKey = "3rcFw0LtVJ0e0Ge58nv9En0J3oVGOpsFpWnVoY+ZFEz2WI3qrt9DbezjCkeIquReI1Any+7uULmaWmzDfdUgCA==";

            //Get a reference to the storage account, with authentication credentials
            StorageCredentials credentials = new StorageCredentials(accountName, accountKey);
            CloudStorageAccount storageAccount = new CloudStorageAccount(credentials, true);

            //Create a new client object.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container. 
            CloudBlobContainer container = blobClient.GetContainerReference("stock");

            CloudBlockBlob blockBlob = container.GetBlockBlobReference("myblob1.txt");
            blockBlob.Properties.ContentType = "application/json";

            // Create or overwrite the "myblob" blob with contents from a local file.
            using (var fileStream = GenerateStreamFromString(content))
            {
                blockBlob.UploadFromStream(fileStream);
            }


        }

        public Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


        private int toSendQty(int current_Stock, int initial_value)
        {
            int tempnewbatch;
            if (initial_value == 0) //Product is taken off the shelf and does not exist for sale
            {
                return 0;
            }
            double time_val = (double)(initial_value - current_Stock) / (double)initial_value; //not initial value/2 as we do this for 15 days
            int newbatch;
            if (time_val >= 0.9)
            {
                tempnewbatch = (int)(time_val - 0.9) * initial_value + initial_value;
            }
            else
            {
                tempnewbatch = (int)(1 + time_val) * initial_value / 2;
            }
            if ((tempnewbatch - current_Stock) > 50)
                newbatch = tempnewbatch - current_Stock; //minimum batch is of 50 items
            else
                newbatch = 0;
            return newbatch;
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
