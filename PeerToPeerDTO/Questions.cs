using System;
using System.ComponentModel.DataAnnotations;

namespace PeerToPeerDTO
{
    public class Questions
    {
        public int ID { get; set; }

        [StringLength(500)]
        public string Question { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public int Rating { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
