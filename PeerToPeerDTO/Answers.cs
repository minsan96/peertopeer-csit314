using System;
using System.ComponentModel.DataAnnotations;

namespace PeerToPeerDTO
{
    public class Answers
    {
        public int ID { get; set; }

        public int QuestionID { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public int Rating { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
