using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using System.Net;
using System.Web.Mvc;

namespace MyEvernote.WebUI.Controllers
{
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
            return RedirectToAction("Index");
        }
    }
}