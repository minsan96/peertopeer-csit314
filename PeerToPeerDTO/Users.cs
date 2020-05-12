using System;
using System.ComponentModel.DataAnnotations;

namespace PeerToPeerDTO
{
    public class Users
    {
        public int ID { get; set; }

        [StringLength(500)]
        public string UserName { get; set; }

        [StringLength(200)]
        public string UserType { get; set; }

        [StringLength(500)]
        public string FirstName { get; set; }
        
        [StringLength(500)]
        public string LastName { get; set; }

        [StringLength(200)]
        public string Password { get; set; }

        public int Rating { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? LastLogin { get; set; }
    }
}
