using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace CARTS.Controllers
{
    public class ManageOrderController : Controller
    {
        private readonly Models.CartsEntities _db = new Models.CartsEntities();
        private readonly Models.UserEntities _db1 = new Models.UserEntities();
        private const int PageSize = 1;

        private IEnumerable<Models.Order> UserName
        {
            get { return _db.Orders.OrderBy(x => x.Id); }
        }
        // GET: ManageOrder
        public ActionResult Index(int page = 1)
        {
            var query = _db.Orders.OrderBy(x => x.Id);
            int pageIndex = page < 1 ? 1 : page;

            //using (Models.CartsEntities db = new Models.CartsEntities())
            //{
            //    int currentPage = page < 1 ? 1 : page;
            //    var Orders = db.Orders.OrderBy(x => x.Id);
            //取得Order中所有資料
            //var result = (from s in db.Orders
            //              select s).ToList();
            var model = new Models.ViewModel.OrderListViewModels
            {
                SearchParameter = new Models.ViewModel.OrderSearchModels(),
                PageIndex = pageIndex,
                Orders = query.ToPagedList(pageIndex, PageSize)
            };
            return View(model);
            //}
        }
        [HttpPost]
        public ActionResult Index(Models.ViewModel.OrderListViewModels model)
        {
            var query = _db.Orders.AsQueryable();
            //var queryUser = _db1.AspNetUsers.AsQueryable();
            //判斷是否為空字串
            if (!string.IsNullOrWhiteSpace(model.SearchParameter.UserID))
            {
                List<string> listUserid = null;
                using (Models.UserEntities db = new Models.UserEntities())
                {   //查詢目前網站使用者暱稱類似UserName的UserId
                    listUserid = (from s in db.AspNetUsers
                                  where s.UserName.Contains(model.SearchParameter.UserID)
                                  select s.Id).ToList();
                }

                List<Models.Order> OrderList = new List<Models.Order>();
                foreach (var listcount in listUserid)
                {  //則將此UserId的所有訂單找出
                    var Order = (from s in _db.Orders
                                 where s.UserId == listcount
                                 select s).ToList();
                    OrderList.AddRange(Order);
                }

                query = OrderList.AsQueryable();
            }

            query = query.OrderBy(x => x.Id);
            int pageIndex = model.PageIndex < 1 ? 1 : model.PageIndex;

            var result = new Models.ViewModel.OrderListViewModels
            {
                SearchParameter = model.SearchParameter,
                PageIndex = model.PageIndex < 1 ? 1 : model.PageIndex,
                Orders = query.ToPagedList(pageIndex, PageSize)
            };
            return View(result);
        }



        public ActionResult Details(int id)
        {
            using (Models.CartsEntities db = new Models.CartsEntities())
            {
                //取得OrderId為傳入id的所有商品列表
                var result = (from s in db.OrderDetails
                              where s.OrderId == id
                              select s).ToList();

                if (result.Count == 0)
                {   //如果商品數目為零，代表該訂單異常(無商品)，則導回商品列表
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(result);
                }
            }
        }

        public ActionResult SerachByUserName(string UserName)
        {
            int currentPage = 1;
            Models.CartsEntities Cartsdb = new Models.CartsEntities();
            var query = Cartsdb.Orders.AsQueryable();



            //儲存查詢出來的UserId
            List<Models.Order> OrderList = new List<Models.Order>();
            List<string> listUserid = null;
            using (Models.UserEntities db = new Models.UserEntities())
            {   //查詢目前網站使用者暱稱類似UserName的UserId
                listUserid = (from s in db.AspNetUsers
                        where s.UserName.Contains(UserName)
                        select s.Id).ToList();
            }
            //如果有相同的UserID存在
            if (listUserid != null)
            {   //則將此UserId的所有訂單找出
                foreach (var listcount in listUserid)
                {
                    query = query.Where(x => x.UserId == listcount);
                        var Order = (from s in Cartsdb.Orders
                                     where s.UserId == listcount
                                     select s).ToList();

                        OrderList.AddRange(Order);
                       
                    
                }
                //回傳 結果 至Index()的View
                return View("Index", OrderList);
            }
            else
            {   //回傳 空結果 至Index()的View
                return View("Index", new List<Models.Order>());
            }

        }


    }
}