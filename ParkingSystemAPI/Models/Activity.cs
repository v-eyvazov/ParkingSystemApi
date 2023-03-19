using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingSystemAPI.Models
{
    [Table("activities")]
    public class Activity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Action cannot be empty.")]
        [Column("action")]
        public string? Action { get; set; }

        [Required(ErrorMessage = "Lot Number cannot be empty.")]
        [Column("ticket_number")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TicketNumber { get; set; }

        [Required(ErrorMessage = "Created time cannot be empty.")]
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }

        [ForeignKey("parking_lot_id")]
        [Required(ErrorMessage = "Foreign key cannot be empty.")]
        public ParkingLot? ParkingLot { get; set; }
    }
}
