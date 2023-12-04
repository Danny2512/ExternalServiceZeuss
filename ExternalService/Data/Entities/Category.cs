using System.ComponentModel.DataAnnotations;

namespace ExternalService.Data.Entities
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string StrName { get; set; }

        [Required]
        public bool BiActive{ get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
