using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyEvernote.Entities
{
    public class MyEntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DisplayName("Oluşturulma Tarihi")]
        [Required]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Güncellenme Tarihi")]
        [Required]
        public DateTime UpdatedDate { get; set; }

        [DisplayName("Oluşturan Kişi")]
        [Required, StringLength(30)]
        public string CreatedUserName { get; set; }
    }
}