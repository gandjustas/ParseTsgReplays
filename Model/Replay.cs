using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ParseTsgReplays
{
    public class Replay
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(2)]
        public string Server { get; set; }
        [MaxLength(50)]
        public string Island { get; set; }
        [MaxLength(50)]
        public string Mission { get; set; }
        public bool IsFinished { get; set; }
        public DateTime Timestamp { get; set; }
        public ICollection<Unit> Units { get; private set; }
        public ICollection<EnterExitEvent> Events { get; private set; }
        public ICollection<Kill> Kills { get; private set; }

        public Replay()
        {
            Units = new HashSet<Unit>();
            Events = new HashSet<EnterExitEvent>();
            Kills = new HashSet<Kill>();
        }
    }
}
