using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParseTsgReplays
{
    public class Unit
    {
        public int ReplayId { get; set; }

        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Class { get; set; }
        public int Side { get; set; }

        [MaxLength(50)]
        public string Icon { get; set; }

        [MaxLength(50)]
        public string Squad { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        public double Damage { get; set; }

        public int? TimeOfDeath { get; set; }

        public bool IsVehicle { get; set; }

        public int? VehicleOrDriverId { get; set; } //at death   
    }
}
