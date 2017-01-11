using AspNetIdentityDemo.Models;
using System.Collections.Generic;
using System.Data.Entity;

namespace AspNetIdentityDemo.DAL
{
    public class IdentityInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Email = "panzhenhua@etutor.cn", EmailConfirmed = false,
                    PasswordHash = "ALf397V4ZYyH2v4ajFt8s6r26HWT1D8RkIlWGqc7Vjztkjn0K5sPuMoU8tTt67wSVg==",
                    SecurityStamp = "2def15ff-b720-4cd4-8e66-c5d4505ef48a", PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false, LockoutEnabled = false, AccessFailedCount = 0, UserName = "panzhenhua@etutor.cn" },
                new ApplicationUser { Email = "lisi@etutor.cn", EmailConfirmed = false,
                    PasswordHash = "ALf397V4ZYyH2v4ajFt8s6r26HWT1D8RkIlWGqc7Vjztkjn0K5sPuMoU8tTt67wSVg==",
                    SecurityStamp = "2def15ff-b720-4cd4-8e66-c5d4505ef48a", PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false, LockoutEnabled = false, AccessFailedCount = 0, UserName = "lisi@etutor.cn" }
            };
            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();
        }
    }
}