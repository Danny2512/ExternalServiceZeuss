using System.ComponentModel.DataAnnotations;

namespace ExternalService.Data.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string StrUserName { get; set; }

        [Required]
        public byte[] HsPassword { get; set; }
    }
}
