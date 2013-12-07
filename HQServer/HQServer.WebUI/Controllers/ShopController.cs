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

       /* public ActionResult TestCharts()
        {
            DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("chart")
                .SetXAxis(new XAxis
                {
                    Categories = new[] { "Jan", "Feb", "Mar", "Apr", "May" }
                })
                .SetSeries(new Series
                {
                    Type = ChartTypes.Pie,
                    //Name =  { "Jan", "Feb", "Mar", "Apr", "May" },

                    Data = new Data(new object[] { 29.9, 71.5, 106.4, 129.2, 45 })
                });

            return View(chart);
        } */

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
                outletTransactionDetail.cost = (float)t["Value"]["unitPrice"];
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

            JArray transactionArray = (JArray)raw["Inventory"];
            OutletInventory outletInventory;

            var outletInventoryList = new Dictionary<Tuple<int, string>, OutletInventory>();

            foreach (var item in _outletInventoryRepo.OutletInventories)
            {
                Tuple<int, string> t = new Tuple<int, string>(item.outletID, item.barcode);
                outletInventoryList.Add(t, item);
            }


            foreach (var t in transactionArray)
            {
                string barcode = (string)t["barcode"];
                int currentStock = (int)t["currentStock"];
                int discountPercentage = (int)t["discount"];
                int minimumStock = (int)t["minimumStock"];
                float sellingPrice = (float)t["sellingPrice"];


                Tuple<int, string> k = new Tuple<int, string>(outletID, barcode);
                if (outletInventoryList.ContainsKey(k))
                    outletInventory = outletInventoryList[k];
                else
                    outletInventory = new OutletInventory();

                outletInventory.outletID = outletID;
                outletInventory.barcode = barcode;
                outletInventory.currentStock = currentStock;
                outletInventory.discountPercentage = discountPercentage;
                outletInventory.minimumStock = minimumStock;
                outletInventory.sellingPrice = sellingPrice;
                _outletInventoryRepo.quickSaveOutletInventory(outletInventory);
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
        public ActionResult InventoryForOutlet(int outletID = 0)
        {
            OutletInventoryViewModel viewModel = new OutletInventoryViewModel();
            String noResults = "No inventory found for ";

            if (outletID != 0 && outletID != null)
            {
                viewModel.Inventory = _outletInventoryRepo.OutletInventories.Where(t => t.outletID == outletID);
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
            DateTime dt = DateTime.Parse(date);
            int id = Int32.Parse(shopID);
            var tid = _outletTransactionRepo.OutletTransactions.First(o => o.outletID == id && dt.Day == o.date.Day && dt.Month == o.date.Month && dt.Year == o.date.Year).transactionSummaryID;

            var outletTransactionDetails = _outletTransactionDetailRepo.OutletTransactionDetails.Where(o => o.transactionSummaryID == tid).ToDictionary(t => t.barcode);

            //var outletTransactionDetailsbyShop = _outletTransactionDetailRepo.OutletTransactionDetails.ToDictionary(t=>t.outletID);

            var fullDetails = _outletTransactionDetailRepo.OutletTransactionDetails.ToDictionary(t => t.outletID.ToString() + t.barcode.ToString());

            var shopinventory = _outletInventoryRepo.OutletInventories.Where(o => o.outletID == id).ToArray();
            var shops = _outletRepo.Outlets.ToArray();
            //var inventory = _outletInventoryRepo.OutletInventories.ToDictionary(s =>(s.outletID.ToString()+s.barcode.ToString()));
            var productDetails = _productRepo.Products.ToDictionary(p => p.barcode);

            Dictionary<string, double> priceList = new Dictionary<string, double>();
            Dictionary<string, double> values = new Dictionary<string, double>();

            var productList = _productRepo.Products.ToArray();

            foreach (var p in productList)
            {
                double value = 0;
                foreach (var shop in shops)
                {
                    try
                    {
                        value = value + fullDetails[shop.outletID.ToString() + p.barcode.ToString()].cost;

                    }
                    catch
                    {
                        //values[p.barcode.ToString()] = 0;
                    }

                }
                values[p.barcode.ToString()] = value;
            }

            int i = 0;
            foreach (var product in shopinventory)
            {

                try
                {
                    double newPrice;
                    if (outletTransactionDetails.ContainsKey(product.barcode))
                    {
                        newPrice = activePrice(product.sellingPrice, product.currentStock, product.minimumStock, outletTransactionDetails[product.barcode].unitSold, values[product.barcode.ToString()], 10000, productDetails[product.barcode.ToString()].costPrice);
                        newPrice = Math.Ceiling(newPrice / .05) * .05;
                        priceList.Add(product.barcode.ToString(), newPrice);
                    }
                    else
                    {
                        newPrice = activePrice(product.sellingPrice, product.currentStock, product.minimumStock, 0, values[product.barcode.ToString()], 10000, productDetails[product.barcode.ToString()].costPrice);
                        newPrice = Math.Ceiling(newPrice / .05) * .05;
                        priceList.Add(product.barcode.ToString(), newPrice);
                   
                    }
                }
                catch { ;}
            }


            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(priceList),
                ContentType = "application/json",
            };
        }

        public double activePrice(double cur_selling_price, int curr_qty, int threshold, int units_sold, double global_sales_value, int max_stock, double cost_price)
        {
            double new_selling_price;
            if ((global_sales_value == 0) || (units_sold == 0))
            {
                new_selling_price = (3 * cur_selling_price) / 4;
                return new_selling_price;
            }
            double value_quo = (cur_selling_price * units_sold) / global_sales_value;
            if (value_quo == 1)
            {
                value_quo /= 10;
            }
            else 
            {
                value_quo /= 5;
            }
            if (curr_qty < 1.1 * threshold)
                new_selling_price = 1.25 * cost_price;

            else if (curr_qty < 1.5 * threshold)
            {
                new_selling_price = cur_selling_price + value_quo * value_quo * cur_selling_price;
            }

            else if (curr_qty >= 0.9 * max_stock)
            {
                new_selling_price = cur_selling_price - value_quo * cur_selling_price;
            }
            else
            {
                new_selling_price = cur_selling_price - value_quo * value_quo * cur_selling_price;
            }
            return new_selling_price;
        }
    }
}
