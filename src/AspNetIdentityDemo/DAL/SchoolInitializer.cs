using System.Collections.Generic;
using System.Data.Entity;

namespace AspNetIdentityDemo.DAL
{
    public class SchoolInitializer : DropCreateDatabaseIfModelChanges<SchoolContext>
    {
        protected override void Seed(SchoolContext context)
        {
            var students = new List<Student>
            {
                new Student { FirstName = "Peter", LastMidName = "Pan", Age = 22 },
                new Student { FirstName = "Jerry", LastMidName = "Xie", Age = 22 }
            };
            students.ForEach(s => context.Students.Add(s));
            context.SaveChanges();
        }
    }
}