using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CloudQuiz.Models
{
    public class QuizContext : IdentityDbContext<ApplicationUser>
    {
        public QuizContext()
            : base("QuizContext") { }

        public static QuizContext Create()
        {
            return new QuizContext();
        }        
        public DbSet<Choice> Choices { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Result> Results { get; set; }
    }

    /*public class DBInitializer : CreateDatabaseIfNotExists<QuizContext>
    {
        protected override void Seed(QuizContext db)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            var role1 = new ApplicationRole { Name = "Admin" };
            var role2 = new ApplicationRole { Name = "User" };
            roleManager.Create(role1);
            roleManager.Create(role2);
            var admin = new ApplicationUser { UserName = "admin" };
            string password = "admin123";
            var result = userManager.Create(admin, password);
            if (result.Succeeded)
            {
                userManager.AddToRole(admin.Id, role1.Name);
                userManager.AddToRole(admin.Id, role2.Name);
            }

            base.Seed(db);
        }
    }*/
}