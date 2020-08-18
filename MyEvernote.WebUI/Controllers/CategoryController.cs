using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using MyEvernote.WebUI.Filters;
using MyEvernote.WebUI.Models;
using System.Net;
using System.Web.Mvc;

namespace MyEvernote.WebUI.Controllers
{
    [Auth]
    [AuthAdmin]
    [Exc]
    public class CategoryController : Controller
    {
        private CategoryManager cm = new CategoryManager();

        public ActionResult Index()
        {
            return View(cm.List());
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = cm.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category category)
        {
            ModelState.Remove("CreatedUserName");
            if (ModelState.IsValid)
            {
                cm.Insert(category);
                CacheHelper.RemoveCategoriesFromCache();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = cm.Find(x => x.Id == id);

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            ModelState.Remove("CreatedUserName");
            if (ModelState.IsValid)
            {
                Category categoreis = cm.Find(x => x.Id == category.Id);
                categoreis.Title = category.Title;
                categoreis.Description = category.Description;

                cm.Update(categoreis);
                CacheHelper.RemoveCategoriesFromCache();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = cm.Find(x => x.Id == id);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Category category = cm.Find(x => x.Id == id);
            cm.Delete(category);
            CacheHelper.RemoveCategoriesFromCache();
            return RedirectToAction("Index");
        }

        
    }
}