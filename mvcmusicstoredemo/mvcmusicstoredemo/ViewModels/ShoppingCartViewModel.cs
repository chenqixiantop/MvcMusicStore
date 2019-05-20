using MvcMusicStoreDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStoreDemo.ViewModels
{
    //ShoppingCartViewModel 将会用于用户的购物车
    public class ShoppingCartViewModel
    {
        //CartItem列表
        public List<Cart> CartItems { get; set; }
        //购物中的总价
        public decimal CartTotal { get; set; }
    }
}