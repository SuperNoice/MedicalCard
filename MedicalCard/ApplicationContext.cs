using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore;
using MedicalCard.Models;

namespace MedicalCard
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Card>? Cards { get; set; }

        public ApplicationContext()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source=./medical.db");
    }
}
