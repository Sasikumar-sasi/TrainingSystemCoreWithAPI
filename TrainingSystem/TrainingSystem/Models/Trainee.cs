using System.ComponentModel.DataAnnotations;

namespace TrainingSystem.Models
{
    public class Trainee
    {
        [Key]
        public int TraineeID { get; set; }
        [Required,MaxLength(30)]
        public string TraineeName { get; set; }
        [Required,MaxLength(15)]
        public string PhoneNumber { get; set; }
        [Required,MaxLength(30)]
        public string MappedTo { get; set; } = "Not Mapped";
        [Required,MaxLength(30)]
        public string EmailId { get; set; }
        [Required, MaxLength(15)]
        public string Password { get; set; }
    }
}
