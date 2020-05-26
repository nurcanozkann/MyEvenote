using MyEvernote.BusinessLayer;
using MyEvernote.BusinessLayer.Results;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ViewModel;
using MyEvernote.WebUI.ViewModel;
using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private EvernoteUserManager evernoteUserManager = new EvernoteUserManager();
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();

        // GET: Home
        public ActionResult Index()
        {
            //BusinessLayer.Test test = new BusinessLayer.Test();
            //test.InsertTest();
            //test.UpdateTest();
            //test.DeleteTest();
            //test.CommentTest();

            return View(noteManager.List().OrderByDescending(x => x.UpdatedDate).ToList());
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category cat = categoryManager.Find(x => x.Id == id.Value);

            if (cat == null)
            {
                return HttpNotFound();
            }

            return View("Index", cat.Notes.OrderByDescending(x => x.UpdatedDate).ToList());
        }

        public ActionResult MostLiked()
        {
            return View("Index", noteManager.List().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(LoginViewModel request)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.LoginUser(request);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.message));

                    if (res.Errors.Find(x => x.code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLik = "http://Home/Activa/1234-5678-8987";
                    }
                    return View(request);
                }

                Session["login"] = res.Result;
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult ShowProfile()
        {
            EvernoteUser currentUser = Session["login"] as EvernoteUser;
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.GetUserById(currentUser.Id);

            if (res.Errors.Count() > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }

        public ActionResult EditProfile()
        {
            EvernoteUser user = Session["login"] as EvernoteUser;
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.GetUserById(user.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }

        [HttpPost]
        public ActionResult EditProfile(EvernoteUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("CreatedUserName");

            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                    (ProfileImage.ContentType == "image/jpeg" ||
                    ProfileImage.ContentType == "image/jpg" ||
                    ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFileName = filename;
                }

                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi.",
                        RedirectingUrl = "/Home/EditProfile"
                    };

                    return View("Error", errorNotifyObj);
                }

                // Profil güncellendiği için session güncellendi.
                Session["login"] = res.Result;
                //CurrentSession.Set<EvernoteUser>("login", res.Result);

                return RedirectToAction("ShowProfile");
            }
            return View(model);
        }

        public ActionResult DeleteProfile()
        {
            EvernoteUser currentUser = Session["login"] as EvernoteUser;
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.RemoveUserById(currentUser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel model = new ErrorViewModel()
                {
                    Title = "Profile Silinemedi",
                    Items = res.Errors,
                    RedirectingUrl = "/Home/ShowProfile"
                };

                return View("Error", model);
            }

            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel request)
        {
            //DataAnnoutation lara verdiğimiz şartlar doğrumu
            if (ModelState.IsValid)
            {
                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.RegisterUser(request);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.message));
                    return View(request);
                }

                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login",
                };
                notifyObj.Items.Add("Lütfen E-posta adresinize  gönderdiğimiz aktivasyon link'ine tıklayarak hesabınızı aktif ediniz.Hesabınızı aktif etmeden not ekleyemez ve beğenme yapamazsınız.");

                return View("Ok", notifyObj);
            }
            return View(request);
        }

        public ActionResult UserActivate(Guid id)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res = evernoteUserManager.ActivateUser(id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hesap Geçerli Değil",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
            }

            OkViewModel okNotifObj = new OkViewModel()
            {
                Title = "Hesap Aktifleştirildi",
                RedirectingUrl = "/Home/Login",
            };

            okNotifObj.Items.Add("  Lütfen E-posta adresinize  gönderdiğimiz aktivasyon link'ine tıklayarak hesabınızı aktif ediniz.Hesabınızı aktif etmeden not ekleyemez ve beğenme yapamazsınız.");

            return View("Ok", okNotifObj);
        }

        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }
    }
}