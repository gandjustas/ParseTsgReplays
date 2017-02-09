using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ParseTsgReplays
{
    public class EnterExitEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ReplayId { get; set; }

        public int Time { get; set; }

        public int UnitId { get; set; }

        public bool IsEnter { get; set; }

        [MaxLength(50)]
        public string User { get; set; }

    }
}
