using MvcMusicStoreDemo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMusicStoreDemo.Models
{
    public class ShoppingCart
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();

        string ShoppingCartId { get; set; }

        public const string CartSessionKey = "CartId";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        private string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    //无用户信息时需要生成GUID序列号
                    Guid tempCartId = Guid.NewGuid();
                    //将tempCartId作为cookie发送到客户端
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        //帮助方法简化购物车电话
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        //AddToCart将专辑作为参数加入到购物车中，在cart表中跟踪每个专辑的数量。
        public void AddToCart(Album album)
        {
            var cartItem = storeDB.Carts.SingleOrDefault
                (c => (c.CartId == ShoppingCartId) && (c.AlbumId == album.AlbumId));

            if (cartItem == null)
            {
                //如果没有购物项目存在则创建一个新的购物项目
                cartItem = new Cart
                {
                    AlbumId = album.AlbumId,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                storeDB.Carts.Add(cartItem);
            }
            else
            {
                //如果存在购物项目，加1
                cartItem.Count++;
            }
            storeDB.SaveChanges();
        }

        //RemoveFromCart，通过专辑的标识从用户的购物车中将这个专辑的数量减少 1，如果用户仅仅剩下一个，那么就删除这一行。
        public int RemoveFromCart(int id)
        {
            //get the cart
            var cartItem = storeDB.Carts.Single
                (cart => cart.CartId == ShoppingCartId && cart.RecordId == id);

            int itemCount = 0;
            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                //如果只剩下一个
                else
                {
                    storeDB.Carts.Remove(cartItem);
                }
                storeDB.SaveChanges();
            }
            return itemCount;
        }


        //EmptyCart，删除用户购物车中所有的项目。
        public void EmptyCart()
        {
            var cartItems = storeDB.Carts.Where
                (cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                storeDB.Carts.Remove(cartItem);
            }
            storeDB.SaveChanges();
        }

        //GetCartItems，获取购物项目的列表用来显示或者处理。
        public List<Cart> GetCartItems()
        {
            return storeDB.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            int? count = (from cartItems in storeDB.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            return count ?? 0;
        }

        public decimal GetTotal()
        {
            decimal? total = (from cartItems in storeDB.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count * cartItems.Album.Price).Sum();
            return total ?? decimal.Zero;
        }

        //将购物车转换为结账处理过程中的订单
        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            var cartItems = GetCartItems();
            //迭代购物车中的商品，添加每个商品的订单详情
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    AlbumId = item.AlbumId,
                    OrderId = order.OrderId,
                    UnitPrice = item.Album.Price,
                    Quantity = item.Count
                };
                //设置购物车的订单总数
                orderTotal += (item.Count * item.Album.Price);
                storeDB.OrderDetails.Add(orderDetail);
            }
            //设置订单的总数量到order Total数量
            order.Total = orderTotal;
            //保存订单
            storeDB.SaveChanges();
            //清空购物车
            EmptyCart();
            return order.OrderId;
        }


        //当用户登录后，将其购物车迁移到与其用户名关联
        public void MigrateCart(string username)
        {
            var shoppingCart = storeDB.Carts.Where(c => c.CartId == ShoppingCartId);
            foreach (Cart item in shoppingCart)
            {
                item.CartId = username;
            }
            storeDB.SaveChanges();
        }
    }
}