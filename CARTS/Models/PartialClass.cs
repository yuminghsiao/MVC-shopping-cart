﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CARTS.Models
{
    public class PartialClass
    {

    }

    //定義Models.Order的部分類別
    public partial class Order
    {
        //取得訂單中的,使用者暱稱
        public string GetUrderName()
        {
            //使用Order類別中的UserID到AspNetUsers資料表中搜尋UserName
            using (Models.UserEntities db = new UserEntities())
            {
                var result = (from s in db.AspNetUsers where s.Id == this.UserId select s.UserName).FirstOrDefault();

                //回傳找到的UserName
                return result;
            }
        }
    }

    //定義Models.ProductCommet的部分類別
    public partial class ProductCommet
    {
        //取得訂單中的,使用者暱稱
        public string GetUrderName()
        {
            //使用Order類別中的UserID到AspNetUsers資料表中搜尋UserName
            using (Models.UserEntities db = new UserEntities())
            {
                var result = (from s in db.AspNetUsers where s.Id == this.UserId select s.UserName).FirstOrDefault();

                //回傳找到的UserName
                return result;
            }
        }
    }
}