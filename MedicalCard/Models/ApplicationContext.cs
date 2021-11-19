using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MedicalCard.Models;
using System.Data.Entity;
using System.Data.SQLite;

namespace MedicalCard
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }

        public ApplicationContext() : base("DefaultConnection")
        {
            
        }

    }
}
