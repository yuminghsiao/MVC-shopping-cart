using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CARTS.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Models.OrderModel.Ship postback)
        {
            if (this.ModelState.IsValid)
            { 
            //取得目前購物車
            var currentcart = Models.Carts.Operation.GetCurrentCart();

            //取得目前登入使用者id
            var userid = HttpContext.User.Identity.GetUserId();

                using (Models.CartsEntities db = new Models.CartsEntities())
                {
                    //建立order物件
                    var order = new Models.Order()
                    {
                        UserId = userid,
                        RecieverName = postback.RecieverName,
                        RecieverPhone = postback.RecieverPhone,
                        RecieverAddress = postback.RecieverAddress
                    };

                    //加其入Order資料表後,儲存後變更
                    db.Orders.Add(order);
                    db.SaveChanges();

                    //取得購物車中的OrderDetails物件
                    var orderDetails = currentcart.ToOrderDetailList(order.Id);
                    //將其加入OrderDetails資料表後儲存
                    db.OrderDetails.AddRange(orderDetails);
                    db.SaveChanges();
                    return Content("訂購成功");
                }
            }
            return View();
        }

        public ActionResult MyOrder()
        {
            //取得目前登入者的id
            var Userid = HttpContext.User.Identity.GetUserId();

            using (Models.CartsEntities db = new Models.CartsEntities())
            {
                var result = (from s in db.Orders where s.UserId == Userid select s).ToList();
                return View(result);
            }
        }

        public ActionResult MyOrderDetails(int id)
        {

            using (Models.CartsEntities db = new Models.CartsEntities())
            {
                var result = (from s in db.OrderDetails where s.OrderId == id select s).ToList();
                if (result.Count == 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(result);
                }
               
            }
        }
    }
}