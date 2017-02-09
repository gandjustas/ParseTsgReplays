using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ParseTsgReplays
{
    public class Kill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KillId { get; set; }
        public int ReplayId { get; set; }
        public int Time { get; set; }
        public int KillerId { get; set; }
        public int TargetId { get; set; }
        [MaxLength(50)]
        public string Weapon { get; set; }
        [MaxLength(50)]
        public string Ammo{ get; set; }
        public double Distance { get; set; }

        public int? KillerVehicleId { get; set; }


    }
}
