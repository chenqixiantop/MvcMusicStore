using mvcmusicstoredemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace mvcmusicstoredemo.Controllers
{
    public class StoreController : Controller
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();
        // GET: Store
        public ActionResult Index()
        {
            //var genres = new List<Genre>
            //{
            //    new Genre{ Name = "Disco"},
            //    new Genre{ Name = "Jazz"},
            //    new Genre{ Name = "Rock"}
            //};
            //var genres = storeDB.Genres.ToList();
            var albums = storeDB.Albums.Include("Genre").Include("Artist");
            ViewBag.Message = "Home page";
            return View(albums);
        }

        public ActionResult Browse(string genre)
        {
            //string message = HttpUtility.HtmlEncode("Store.Browse,Genre = " + genre);
            //return message;
            var genreModel = storeDB.Genres.Include("Albums")
                .Single(g => g.Name == genre);
            return this.View(genreModel);
        }

        public ActionResult Details(int? id)
        {
            //var album = new Album { Title = "Album " + id };
            //return View(album);
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Album album = storeDB.Albums.Find(id);
            if(album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        //Get 方式的 Action 用来提供编辑表单，而 Post 方式的 Action 用来获取用户提交的数据
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = storeDB.Albums.Find(id);
            if(album == null)
            {
                return HttpNotFound();
            }
            ViewBag.GenreId = new SelectList(storeDB.Genres, "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(storeDB.Artists, "ArtistId", "Name", album.Artist);
            return View(album);
        }

        //表单的提交方式设置为 Post 方式，用户在提交表单的时候，将填写的数据提交到服务器
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include= "AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Album album)
        {
            if(ModelState.IsValid) //模型状态是否有效
            {
                //将这个对象的状态属性 State 修改为 Modified ，这就回告诉 EF 我们正在修改一个存在的专辑对象，而不是创建一个新的
                storeDB.Entry(album).State = System.Data.Entity.EntityState.Modified;
                storeDB.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GenreId = new SelectList(storeDB.Genres, "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(storeDB.Artists, "ArtistId", "Name", album.Artist);
            return View(album);
        }

        //GET:/Album/Delete/5
        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = storeDB.Albums.Find(id);
            if(album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: /Album/Delete/5
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Album album = storeDB.Albums.Find(id);
            storeDB.Albums.Remove(album);
            storeDB.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET: /Store/Create
        public ActionResult Create()
        {
            ViewBag.GenreId = new SelectList(storeDB.Genres, "GenreId", "Name");//流派对象的集合
            ViewBag.ArtistId = new SelectList(storeDB.Artists, "ArtistId", "Name");
            return View();
        }

        //POST: /Store/Create
        [HttpPost]
        public ActionResult Create(Album album)
        {
            if(ModelState.IsValid)
            {
                storeDB.Albums.Add(album);
                storeDB.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GenreId = new SelectList(storeDB.Genres, "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(storeDB.Artists, "ArtistId", "Name", album.ArtistId);
            return View(album);
        }
    }
}