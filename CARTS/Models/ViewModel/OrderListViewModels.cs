using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace CARTS.Models.ViewModel
{
    public class OrderListViewModels
    {
        public OrderSearchModels SearchParameter { get; set; }
        public IPagedList<Order> Orders { get; set; }
        public int PageIndex { get; set; }
    }
}