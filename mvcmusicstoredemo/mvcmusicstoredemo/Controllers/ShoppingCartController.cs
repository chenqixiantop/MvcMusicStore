using MvcMusicStoreDemo.Data;
using MvcMusicStoreDemo.Models;
using MvcMusicStoreDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMusicStoreDemo.Controllers
{
    public class ShoppingCartController : Controller
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            //Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(viewModel);
        }

        //GET:/Store/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            var addAlbum = storeDB.Albums.Single
                (album => album.AlbumId == id);
            //把货物ID加入到数据库中
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addAlbum);
            return RedirectToAction("Index");
        }

        //AJAX：/ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            //从购物车中移除
            var cart = ShoppingCart.GetCart(this.HttpContext);
            //拿到唱片的名字去展示信息
            string albumName = storeDB.Carts.Single
                (item => item.RecordId == id).Album.Title;
            //移除购物车
            int itemCount = cart.RemoveFromCart(id);
            //展示信息
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(albumName)
                     + "has been removed from you shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            return Json(results);
        }

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewData["CartCount"] = cart.GetCount();

            return PartialView("CartSummary");
        }
    }
}