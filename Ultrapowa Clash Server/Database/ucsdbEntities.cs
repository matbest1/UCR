using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace UCS.Database
{
    class ucsdbEntities : DbContext
    {
        public ucsdbEntities(string connectionString)
            : base("name=" + connectionString)
        {
        }

        public virtual DbSet<clan> clan { get; set; }

        public virtual DbSet<player> player { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    }
}