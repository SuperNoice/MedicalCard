using MedicalCard.Models;
using System.Data.Entity;

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
