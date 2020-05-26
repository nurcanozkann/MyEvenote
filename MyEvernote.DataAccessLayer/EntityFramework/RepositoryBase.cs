using MyEvernote.DataAccessLayer;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    //Singleton Patter
    //Verdiği hata:System.InvalidOperationException: 'An entity object cannot be referenced by multiple instances of IEntityChangeTracker.'
    //DatabaseContext in sürekli oluşmaması için tek bir kere olusturup sadece onu kullanmasını sağladık.
    //Bir nesnenin sadece proje calısırken bir kopyası olsutursun baska olusturmasın ver her biri sadece o kopyayı kullanması amaclı
    //DatabaseContext; user,comment,likes,category,note hepsi ayrı ayrı new lediği için hata veriyor bunu bir e indireceğiz
    public class RepositoryBase
    {
        protected static DatabaseContext context;
        private static object _lockSync = new object();

        protected RepositoryBase()
        {
            CreatContext();
        }

        private static void CreatContext()
        {
            if (context == null)
            {
                //lock aynı aynı iki thead in calıstırılamayacağını söyler ilk iş biter sonra diğerini yapar
                lock (_lockSync)
                {
                    if (context == null)
                    {
                        context = new DatabaseContext();
                    }
                }
            }
        }
    }
}