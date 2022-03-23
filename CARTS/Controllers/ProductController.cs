using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carts.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            //宣告回傳商品列表result
            List<CARTS.Models.Product> result = new List<CARTS.Models.Product>();

            //接收輔導的成功訊息
            ViewBag.ResultMeesage = TempData["ResultMeesage"];

            //使用CartsEntities類別，名稱為db
            using (CARTS.Models.CartsEntities db = new CARTS.Models.CartsEntities())
            {
                //使用LinQ語法抓取目前Products資料庫中所有資料
                result = (from s in db.Products select s).ToList();

                //將result傳送給檢視
                return View(result);
            }
        }

        //建立商品畫面
        public ActionResult Create()
        {
            return View();
        }

        //建立商品畫面 - 資料回傳處理
        [HttpPost]//指定只有post才可進入
        public ActionResult Create(CARTS.Models.Product postback)
        {
            if (this.ModelState.IsValid)//如果資料驗證成功
            {
                using (CARTS.Models.CartsEntities db = new CARTS.Models.CartsEntities())
                {
                    //加入回傳資料加入Products
                    db.Products.Add(postback);
                    //儲存異動資料
                    db.SaveChanges();

                    //設定成功訊息
                    TempData["ResultMeesage"] = String.Format("商品[{0}]建立成功", postback.Name);

                    //輔導product/Index頁面
                    return RedirectToAction("Index");
                }
            }
            //失敗訊息
            ViewBag.ResultMessage = "資料有誤，請檢查";
            //停留create頁面
            return View(postback);
        }

        public ActionResult Edit(int id)
        {
            using (CARTS.Models.CartsEntities db = new CARTS.Models.CartsEntities())
            {
                //抓取Product.id 等於輸入id的資料
                var result = (from s in db.Products where s.Id == id select s).FirstOrDefault();
                if (result != default(CARTS.Models.Product))//判斷此id 是否有資料
                {
                    return View(result);//回傳編輯商品頁面
                }
                else
                {
                    //如果沒有資料則顯示錯誤訊息並倒回index頁面
                    TempData["ResultMeesage"] = "資料有誤，請重新操作";
                    return RedirectToAction("Index");
                }

            }
        }

        [HttpPost]
        public ActionResult Edit(CARTS.Models.Product postback)
        {
            if (this.ModelState.IsValid)
            {
                using (CARTS.Models.CartsEntities db = new CARTS.Models.CartsEntities())
                {
                    //抓取Product.id 等於回傳postback.id的資料
                    var result = (from s in db.Products where s.Id == postback.Id select s).FirstOrDefault();

                    //儲存使用者變更的資料
                    result.Name = postback.Name;
                    result.Price = postback.Price;
                    result.PublishDate = postback.PublishDate;
                    result.Quantity = postback.Quantity;
                    result.Status = postback.Status;
                    result.CategoryId = postback.CategoryId;
                    result.DefaultImageId = postback.DefaultImageId;
                    result.Description = postback.Description;
                    result.DefaultImageURL = postback.DefaultImageURL;


                    //儲存異動資料
                    db.SaveChanges();

                    //設定編輯成功訊息並回index頁面
                    TempData["ResultMeesage"] = String.Format("商品[{0}]成功編輯", postback.Name);
                    return RedirectToAction("Index");
                }
            }
            else//如果資料不正確則導向自己
            {
                return View(postback);
            }
        }

        //刪除商品
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (CARTS.Models.CartsEntities db = new CARTS.Models.CartsEntities())
            {
                //抓取Product.id 等於輸入id的資料
                var result = (from s in db.Products where s.Id == id select s).FirstOrDefault();
                if (result != default(CARTS.Models.Product))
                {
                    db.Products.Remove(result);//刪除查詢到的資料

                    db.SaveChanges();//儲存變更

                    //設定刪除成功訊息並回index頁面
                    TempData["ResultMeesage"] = String.Format("商品[{0}]成功刪除", result.Name);
                    return RedirectToAction("Index");

                }
                else
                {
                    //如果沒有資料則顯示錯誤訊息並導回index頁面
                    TempData["ResultMeesage"] = "指定資料不存在，無法刪除，請重新操作";
                    return RedirectToAction("Index");
                }
            }
        }
    }
}