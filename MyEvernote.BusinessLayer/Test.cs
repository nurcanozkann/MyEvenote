using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using System;

namespace MyEvernote.BusinessLayer
{
    public class Test
    {
        private Repository<EvernoteUser> repo_user = new Repository<EvernoteUser>();
        private Repository<Comment> repo_comment = new Repository<Comment>();
        private Repository<Note> repo_note = new Repository<Note>();
        private Repository<Category> repo_cat = new Repository<Category>();

        public Test()
        {
            // DB OLUŞTURMA KODU

            #region db create

            //DatabaseContext context = new DatabaseContext();
            ////sadece database'i oluşturur.
            ////context.Database.Create();
            ////Seed datanında calısması için bir şey çağırmak lazım
            //context.Categories.ToList();

            #endregion db create
        }

        public void InsertTest()
        {
            EvernoteUser newUser = new EvernoteUser()
            {
                Name = "Sibel",
                Surname = "Gezer",
                Email = "sibelgezer@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "sibelgezer",
                Password = "111",
                CreatedDate = DateTime.Now.AddMinutes(5),
                UpdatedDate = DateTime.Now.AddMinutes(10),
                CreatedUserName = "sibelgezer"
            };

            repo_user.Insert(newUser);
        }

        public void UpdateTest()
        {
            EvernoteUser name = repo_user.Find(x=>x.Username == "sibelgezer");
            if (name != null)
            {
                name.Username = "mizginbayam";
                repo_user.Update(name);
            }
        }

        public void DeleteTest()
        {
            EvernoteUser username = repo_user.Find(x=>x.Username == "mizginbayam");
            if (username != null)
            {
                repo_user.Delete(username);
            }
        }

        public void CommentTest()
        {
            EvernoteUser owner = repo_user.Find(x=>x.Id == 1);
            Note note = repo_note.Find(x=>x.Id == 3);
            Comment comment = new Comment()
            {
                Text="Bu bir testtir.",
                CreatedDate=DateTime.Now,
                UpdatedDate=DateTime.Now.AddMinutes(5),
                CreatedUserName="canakti",
                Owner=owner,
                Note= note
            };

            repo_comment.Insert(comment);
        }
    }
}