using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyEvernote.Entities
{
    [Table("Categories")]
    public class Category : MyEntityBase
    {
        [DisplayName("Başlık")]
        [Required(ErrorMessage ="{0} alanı gereklidir."), 
        StringLength(50,ErrorMessage ="{0} alanı {1} max. karakter olmalıdır.")]
        public string Title { get; set; }

        [DisplayName("Açıklama")]
        [StringLength(150, ErrorMessage ="{0} alanı {1} max. karakter olmalıdır.")]
        public string Description { get; set; }

        //bir categorynin birden çok notu olacak.
        //virtual: diğer classlarla ilişkisel demek
        public virtual List<Note> Notes { get; set; }

        public Category()
        {
            Notes = new List<Note>();
        }
    }
}