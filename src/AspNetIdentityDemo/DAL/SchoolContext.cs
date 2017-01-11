using System.Data.Entity;

namespace AspNetIdentityDemo.DAL
{
    public class SchoolContext : DbContext
    {
        public SchoolContext() : base("DefaultConnection")
        {

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}