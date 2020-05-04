using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeerToPeerDTO;

namespace FrontEnd.Models
{
    public class QnaContext : DbContext
    {
        public QnaContext (DbContextOptions<QnaContext> options)
            : base(options)
        {
        }

        public DbSet<PeerToPeerDTO.Questions> Questions { get; set; }
    }
}
