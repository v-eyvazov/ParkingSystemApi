using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingSystemAPI.Models
{
    [Table("parking_lots")]
    public class ParkingLot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Lot Number cannot be empty.")]
        [Column("lot_number")]
        public int LotNumber { get; set; }

        [Required(ErrorMessage = "IsAvailable field cannot be empty.")]
        [Column("is_available")]
        public bool IsAvailable { get; set; } = true;

        public IList<Activity> Activities { get; } = new List<Activity>();
    }
}
