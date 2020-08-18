using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.Entities;
using System.Linq;

namespace MyEvernote.BusinessLayer
{
    public class CategoryManager : ManagerBase<Category>
    {
        public override int Delete(Category category)
        {

            //1)Yöntem: Kategori ile ilişkili notların silinmesi gerekiyor.
            // Burada override edildi çünkü uyguulamalar arası köprü gören kısım burası burda olması daha doğru
            //2)Yöntem: Dbtarafında Database Diagramda oluşan yapı üzerinde ilişkili okların üerini tıklayarak properties tarafıdan delete action'ı cascade et.
            //3) Yöntem: Databese in oluştuğu yer (DatabaseContext) buraya OnModelCreating override ederek FluentApi şeklinde oluşturulabilir.

            NoteManager noteManager = new NoteManager();
            LikedManager likedManager = new LikedManager();
            CommentManager commentManager = new CommentManager();

            foreach (Note note in category.Notes.ToList())
            {
                foreach (Liked like in note.Likes.ToList())
                {
                    likedManager.Delete(like);
                }

                foreach (Comment comment in note.Comments.ToList())
                {
                    commentManager.Delete(comment);
                }

                noteManager.Delete(note);
            }

            return base.Delete(category);
        }
    }
}