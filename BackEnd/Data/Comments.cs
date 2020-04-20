using System;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Data
{
    public class Comments
    {
        public int ID { get; set; }

        public int QuestionID { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public int Rating { get; set; }

        public bool MarkAsAnswer { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
