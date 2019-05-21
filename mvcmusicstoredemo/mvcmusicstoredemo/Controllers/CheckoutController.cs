using MvcMusicStoreDemo.Data;
using MvcMusicStoreDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMusicStoreDemo.Controllers
{
    //这一步很像我们前面在 StoreManager 控制器上的工作，
    //但是，在那个时候，我们要求用户必须拥有 Administrator 的角色。
    //在结账控制器中，我们不需要用户必须是 Administrator ，而是必须登录。
    [Authorize]
    public class CheckoutController : Controller
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();
        const string PromoCode = "FREE";


        //结账的控制器将包含下面的控制器方法：
        //AddressAndPayment(Get ) 用来显示一个用户输入信息的表单
        //AddressAndPayment(Post ) 验证用户的输入，处理订单。
        //Complete 用来在在用户完成订单之后显示，这个视图包含用户的订单账号和确认信息。
        //首先，将 Index 方法改名为 AddressAndPayment， 这个 Action 方法用来显示结账表单，所以，不需要任何的模型信息。

        // GET: Checkout
        public ActionResult AddressAndPayment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);
            try
            {
                if (string.Equals(values["PromoCode"], PromoCode,
                    StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(order);
                }
                else
                {
                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;
                    //save order
                    storeDB.Orders.Add(order);
                    storeDB.SaveChanges();
                    //Process the order
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order);

                    return RedirectToAction("Complete", new { id = order.OrderId });
                }
            }
            catch
            {
                //无效 - 重新显示错误
                return View(order);
            }
        }

        //GET:/Checkout/Complete

        public ActionResult Complete(int id)
        {
            //验证客户拥有此订单
            bool isValid = storeDB.Orders.Any(
                o => o.OrderId == id &&
                o.Username == User.Identity.Name);

            if(isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}