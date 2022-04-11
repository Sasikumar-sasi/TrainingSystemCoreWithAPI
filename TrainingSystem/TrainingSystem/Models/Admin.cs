using System.ComponentModel.DataAnnotations;

namespace TrainingSystem.Models
{
    public class Admin
    {
        [Key]
        public int AdminID { get; set; }
        [Required,MaxLength(30)]
        public string EmailId { get; set; }
        [Required,MaxLength(15)]
        public string Password { get; set; }
    }
}
