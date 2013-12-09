using HQServer.Domain.Abstract;
using HQServer.Domain.Entities;
using HQServer.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;




namespace HQServer.WebUI.Controllers
{
    public class ShopController : Controller
    {
        IProductRepository _productRepo;
        ICategoryRepository _categoryRepo;
        IManufacturerRepository _manufacturerRepo;
        IOutletRepository _outletRepo;
        IOutletTransactionRepository _outletTransactionRepo;
        IOutletTransactionDetailRepository _outletTransactionDetailRepo;
        IOutletInventoryRepository _outletInventoryRepo;

        public ShopController(IProductRepository productRepo, ICategoryRepository categoryRepo,
                              IManufacturerRepository manufacturerRepo, IOutletRepository outletRepo,
                              IOutletTransactionRepository outletTransactionRepo,
                              IOutletTransactionDetailRepository outletTransactionDetailRepo,
                              IOutletInventoryRepository outletInventoryRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _manufacturerRepo = manufacturerRepo;
            _outletRepo = outletRepo;
            _outletTransactionRepo = outletTransactionRepo;
            _outletTransactionDetailRepo = outletTransactionDetailRepo;
            _outletInventoryRepo = outletInventoryRepo;
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
                return RedirectToAction("List");
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

            decimal shop1 = _outletTransactionDetailRepo.OutletTransactionDetails.Where(t => t.outletID == 1).Select(t => t.cost).Sum();
            decimal shop2 = _outletTransactionDetailRepo.OutletTransactionDetails.Where(t => t.outletID == 2).Select(t => t.cost).Sum();
            decimal shop3 = _outletTransactionDetailRepo.OutletTransactionDetails.Where(t => t.outletID == 3).Select(t => t.cost).Sum();
            decimal shop4 = _outletTransactionDetailRepo.OutletTransactionDetails.Where(t => t.outletID == 4).Select(t => t.cost).Sum();
            decimal shop5 = _outletTransactionDetailRepo.OutletTransactionDetails.Where(t => t.outletID == 5).Select(t => t.cost).Sum();
            DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("chart")
                .SetTitle(new Title
                {
                    Text = " Revenue for all shops during the month of September 2013"
                }
                    )
                .SetSeries(new Series
                {
                    Name = "Revenue",
                    Type = ChartTypes.Pie,


                    Data = new Data(new object[] { new object[] { "Shop1", shop1 }, new object[] { "Shop2", shop2 }, new object[] { "Shop3", shop3 }, new object[] { "Shop4", shop4 }, new object[] { "Shop5", shop5 } })
                });

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
                },
                chart = chart
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

        public ViewResult ViewTransactions(int page = 1)
        {
            int PageSize = 300;
            TransactionListViewModel viewModel = new TransactionListViewModel
            {
                Transactions = _outletTransactionRepo.OutletTransactions
                .OrderBy(t => t.transactionSummaryID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _outletTransactionRepo.OutletTransactions.Count()
                }
            };

            return View(viewModel);
        }

        [HttpPost]
        public string UploadTransactions(string input = null)
        {
            var i = 0;
            if (_outletTransactionRepo.OutletTransactions.Count() == 0)
                i = 1;
            else
                i = _outletTransactionRepo.OutletTransactions.Max(t => t.transactionSummaryID) + 1;
            OutletTransaction outletTransaction = new OutletTransaction();
            JObject raw = JObject.Parse(input);
            outletTransaction.date = (DateTime)raw["Date"];
            outletTransaction.outletID = (int)raw["OutletID"];
            outletTransaction.transactionSummaryID = i;
            JArray transactionArray = (JArray)raw["TransactionDetails"];


            foreach (var t in transactionArray)
            {
                OutletTransactionDetail outletTransactionDetail = new OutletTransactionDetail();
                outletTransactionDetail.transactionSummaryID = outletTransaction.transactionSummaryID;
                outletTransactionDetail.barcode = (string)t["Key"];
                outletTransactionDetail.unitSold = (int)t["Value"]["quantity"];
                outletTransactionDetail.cost = (decimal)t["Value"]["unitPrice"];
                outletTransactionDetail.outletID = (int)t["Value"]["outletID"];
                _outletTransactionDetailRepo.quickSaveOutletTransactionDetail(outletTransactionDetail);
            }

            _outletTransactionDetailRepo.saveContext();
            _outletTransactionRepo.saveOutletTransaction(outletTransaction);

            return "SUCCESS";
        }


        [HttpPost]
        public string UploadOutletInventory(string input = null)
        {
            /* var i = 0;

             if (_outletInventoryRepo.OutletInventories.Count() == 0)
                 i = 1;
             else
              ;//   i = _outletInventoryRepo.OutletInventories.OrderByDescending(x => x.).First().transactionSummaryID + 1;
             */
            JObject raw = JObject.Parse(input);
            int outletID = (int)raw["ShopID"];

            JArray inventoryArray = (JArray)raw["Inventory"];
            OutletInventory outletInventory;

            var outletInventoryList = new Dictionary<Tuple<int, string>, OutletInventory>();

            var outletInventoriesArray = _outletInventoryRepo.OutletInventories.ToArray();
            var outletInventoryDictionary = _outletInventoryRepo.OutletInventories.ToDictionary(t => t.outletID.ToString() + t.barcode);
            foreach (var item in outletInventoriesArray)
            {
                Tuple<int, string> t = new Tuple<int, string>(item.outletID, item.barcode);
                outletInventoryList.Add(t, item);
            }


            foreach (var t in inventoryArray)
            {
                string barcode = (string)t["barcode"];
                int currentStock = (int)t["currentStock"];
                int discountPercentage = (int)t["discount"];
                int minimumStock = (int)t["minimumStock"];
                decimal sellingPrice = (decimal)t["sellingPrice"];
                int austock = (int)t["afterUpdateStock"];
                int temporaryStock = (int)t["temporaryStock"];


                Tuple<int, string> k = new Tuple<int, string>(outletID, barcode);
                if (outletInventoryList.ContainsKey(k))
                {
                    outletInventory = outletInventoryDictionary[outletID.ToString() + barcode];
                    //outletInventory.outletID = outletID;
                    //outletInventory.barcode = barcode;
                    outletInventory.currentStock = currentStock;
                    outletInventory.discountPercentage = discountPercentage;
                    outletInventory.minimumStock = minimumStock;
                    outletInventory.sellingPrice = sellingPrice;
                    outletInventory.afterUpdateStock = austock;
                    outletInventory.temporaryStock = temporaryStock;

                    _outletInventoryRepo.quickUpdateOutletInventory(outletInventory);
                }
                else
                {
                    outletInventory = new OutletInventory();

                    outletInventory.outletID = outletID;
                    outletInventory.barcode = barcode;
                    outletInventory.currentStock = currentStock;
                    outletInventory.discountPercentage = discountPercentage;
                    outletInventory.minimumStock = minimumStock;
                    outletInventory.sellingPrice = sellingPrice;
                    _outletInventoryRepo.quickSaveOutletInventory(outletInventory);
                }
            }

            _outletInventoryRepo.saveContext();
            return "SUCCESS";
        }



        public ActionResult TransactionDetails(int transactionSummaryID)
        {
            TransactionDetailsListViewModel viewModel = new TransactionDetailsListViewModel();
            viewModel.transaction = _outletTransactionRepo.OutletTransactions.First(t => t.transactionSummaryID == transactionSummaryID);
            viewModel.TransactionDetail = _outletTransactionDetailRepo.OutletTransactionDetails.Where(td => td.transactionSummaryID == transactionSummaryID);
            if (viewModel.TransactionDetail.Count() == 0)
            {
                TempData["results"] = "Invalid transaction ID";
                return View();
            }
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult ViewTransactionsForOutlet()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TransactionsForOutlet(int outletID = 0)
        {
            TransactionListViewModel viewModel = new TransactionListViewModel();
            String noResults = "No transactions found for ";

            if (outletID != 0 && outletID != null)
            {
                viewModel.Transactions = _outletTransactionRepo.OutletTransactions.Where(t => t.outletID == outletID);
                noResults += "outlet with ID = " + outletID;
            }

            if (viewModel.Transactions.Count() != 0)
                return View("OutletTransactions", viewModel);
            else
            {
                TempData["results"] = noResults;
                return View("ViewTransactionsForOutlet");
            }

        }

        [HttpGet]
        public ActionResult ViewInventoryForOutlet()
        {
            return View();
        }

        [HttpGet]
        public ActionResult InventoryForOutlet(int outletID = 0, int page = 1)
        {
            var barcodeList = _outletTransactionDetailRepo.OutletTransactionDetails.Where(t => t.outletID == outletID).OrderByDescending(t => t.unitSold).Select(t => t.barcode).ToList();
            var costList = _outletTransactionDetailRepo.OutletTransactionDetails.Where(t => t.outletID == outletID).OrderByDescending(t => t.unitSold).Select(t => t.unitSold).ToList();

            DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("chart")
                .SetXAxis(new XAxis
                {
                    Categories = new[] { barcodeList[0], barcodeList[1], barcodeList[2], barcodeList[3], barcodeList[4] }
                }
                    )
                .SetTitle(new Title
                {
                    Text = " Top selling products for outlet  " + outletID + " September 2013"
                }
                    )
                .SetSeries(new Series
                {
                    Name = "Units sold",
                    Type = ChartTypes.Bar,


                    Data = new Data(new object[] { costList[0], costList[1], costList[2], costList[3], costList[4] })
                });

            OutletInventoryViewModel viewModel = new OutletInventoryViewModel();
            String noResults = "No inventory found for ";

            if (outletID != 0 && outletID != null)
            {
                //viewModel.Inventory = _outletInventoryRepo.OutletInventories.Where(t => t.outletID == outletID);
                //viewModel.chart = chart;

                viewModel.Inventory = _outletInventoryRepo.OutletInventories.OrderBy(p => p.outletID).Where(t => t.outletID == outletID).Skip((page - 1) * PageSize).Take(PageSize);
                viewModel.PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _outletInventoryRepo.OutletInventories.Where(t => t.outletID == outletID).Count()
                };
                viewModel.chart = chart;
                viewModel.currentOutletID = outletID;
                noResults += "outlet with ID = " + outletID;
            }

            if (viewModel.Inventory.Count() != 0)
                return View("InventoryForOutlet", viewModel);
            else
            {
                TempData["results"] = noResults;
                return View("ViewInventoryForOutlet");
            }

        }

        public ContentResult getProductDetails(string barcode)
        {

            Product p = _productRepo.Products.First(t => t.barcode == barcode);
            Dictionary<string, object> d = new Dictionary<string, object>();
            if (p != null)
            {
                d.Add("Status", "success");
                d.Add("Product", p);
            }
            else
                d.Add("Status", "Fail");

            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(d),
                ContentType = "application/json",
            };
        }

        public ContentResult getNewPrices(string shopID, string date)
        {
            //DateTime dt = DateTime.Parse(date);
            int id = Int32.Parse(shopID);
            // var tid = _outletTransactionRepo.OutletTransactions.First(o => o.outletID == id && dt.Day == o.date.Day && dt.Month == o.date.Month && dt.Year == o.date.Year).transactionSummaryID;

            // var outletTransactionDetails = _outletTransactionDetailRepo.OutletTransactionDetails.Where(o => o.transactionSummaryID == tid).ToDictionary(t => t.barcode);

            //var outletTransactionDetailsbyShop = _outletTransactionDetailRepo.OutletTransactionDetails.ToDictionary(t=>t.outletID);

            //var fullOutletInventories = _outletInventoryRepo.OutletInventories.ToDictionary(t=>t.outletID.ToString()+t.barcode);

            //var fullDetails = _outletTransactionDetailRepo.OutletTransactionDetails.ToDictionary(t => t.outletID.ToString() + t.barcode.ToString());

            var shopinventory = _outletInventoryRepo.OutletInventories.Where(o => o.outletID == id).ToArray();
            var shops = _outletRepo.Outlets.ToArray();
            var inventory = _outletInventoryRepo.OutletInventories.ToDictionary(s => (s.outletID.ToString() + s.barcode.ToString()));
            var productDetails = _productRepo.Products.ToDictionary(p => p.barcode);

            Dictionary<string, double> priceList = new Dictionary<string, double>();
            Dictionary<string, double> values = new Dictionary<string, double>();

            var productList = _productRepo.Products.ToArray();

            int j = 0;
            foreach (var product in shopinventory)
            {
                if (product.afterUpdateStock == 0)
                    continue;
                j = 0;
                values[product.barcode] = 0;
                foreach (var shop in shops)
                {

                    if (j == 2)
                        break;
                    j++;
                    try
                    {
                        OutletInventory o = inventory[shop.outletID + product.barcode];
                        if (values.ContainsKey(product.barcode))
                            values[product.barcode] += (double)((double)o.temporaryStock - (double)o.currentStock) / (double)(o.temporaryStock);
                        else
                        {
                            values[product.barcode] = (double)((double)o.temporaryStock - (double)o.currentStock) / (double)(o.temporaryStock);
                        }
                    }
                    catch
                    { ; }
                }
            }


            int i = 0;
            foreach (var product in shopinventory)
            {
                if (product.afterUpdateStock == 0)
                    continue;
                try
                {
                    double newPrice;

                    {
                        newPrice = 0;
                        newPrice = activePrice((double)product.sellingPrice, product.currentStock, product.temporaryStock, product.minimumStock, (double)values[product.barcode], 2);
                        newPrice = Math.Ceiling(newPrice / 0.05) * 0.05;
                        priceList.Add(product.barcode.ToString(), newPrice);
                    }

                }
                catch { ;}
            }

            //createBlob(serializer.Serialize(priceList));

            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            createBlob(serializer.Serialize(priceList), shopID);
            return new ContentResult()
            {
                Content = serializer.Serialize(priceList),
                ContentType = "application/json",
            };
        }

        double activePrice(double cur_selling_price, int curr_Stock, int initial_value, int threshold, double global_time_val, int number_of_shops) //Initial value = batchupdate + remaining value ;(after batch arrives) //initial value in the function argument should be updated weekly since active pricing runs weekly
        {
            double new_selling_price;
            double time_val = (double)((initial_value - curr_Stock)) / (double)(initial_value); //time_val denotes the statistical value of sales of a product in a particular shop over a window of 7 days

            if (global_time_val == 0) //global_time_value denotes sum of all such time values for a particular product from different markets. 
            {
                new_selling_price = 0.99 * cur_selling_price;
                return new_selling_price;// This case occurs when there is absolutely no sales of a product in any market
            }

            double value_quo = time_val / global_time_val; //value_quo denotes the sales value of a particular market for a particular product

            if ((time_val == 0) && (value_quo == 0))
            {
                new_selling_price = 0.98 * cur_selling_price;
                return new_selling_price;//This case occurs when a particular market does not have any sale of a product but other markets do
            }
            if (curr_Stock < 1.1 * threshold)
            {
                new_selling_price = 1.25 * cur_selling_price;
                return new_selling_price;//This case occurs when there is too much less stock for reserve in a particular market
            }
            else
            {
                new_selling_price = cur_selling_price + ((value_quo - ((double)(1) / (double)(number_of_shops)) + (time_val - 0.25)) * cur_selling_price / 10); //This case occurs when both time_value and market_value act together to update
            }
            return new_selling_price;
        }

        public void createBlob(string content, string shopID)
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

            CloudBlockBlob blockBlob = container.GetBlockBlobReference("prices" + shopID + ".txt");
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

    }
}