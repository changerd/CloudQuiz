using CloudQuiz.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CloudQuiz.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        QuizContext db = new QuizContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        private ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }
        public ActionResult GetRole()
        {
            return View(RoleManager.Roles);
        }
        public ActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateRole(CreateRoleModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await RoleManager.CreateAsync(new ApplicationRole
                {
                    Name = model.RoleName,
                });
                if (result.Succeeded)
                {
                    return RedirectToAction("GetRole");
                }
                else
                {
                    ModelState.AddModelError("", "Что-то пошло не так");
                }
            }
            return View(model);
        }
        public async Task<ActionResult> EditRole(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                return View(new EditRoleModel { Id = role.Id, RoleName = role.Name });
            }
            return RedirectToAction("GetRole");
        }
        [HttpPost]
        public async Task<ActionResult> EditRole(EditRoleModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole role = await RoleManager.FindByIdAsync(model.Id);
                if (role != null)
                {
                    role.Name = model.RoleName;
                    IdentityResult result = await RoleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("GetRole");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Что-то пошло не так");
                    }
                }
            }
            return View(model);
        }
        public async Task<ActionResult> DeleteRole(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }
        [HttpPost, ActionName("DeleteRole")]
        public async Task<ActionResult> DeleteRoleConfirmed(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            IdentityResult result = await RoleManager.DeleteAsync(role);
            return RedirectToAction("GetRole");


        }
        public async Task<ActionResult> GiveRole(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            string[] memberIDs = role.Users.Select(x => x.UserId).ToArray();

            IEnumerable<ApplicationUser> members = UserManager.Users.Where(x => memberIDs.Any(y => String.Equals(y, x.Id)));

            IEnumerable<ApplicationUser> nonMembers = UserManager.Users.Except(members);

            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<ActionResult> GiveRole(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    result = await UserManager.AddToRoleAsync(userId, model.RoleName);

                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    result = await UserManager.RemoveFromRoleAsync(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                return RedirectToAction("GetRole");
            }
            return View("Error", new string[] { "Роль не найдена" });
        }
        public ActionResult GetUser()
        {
            return View(UserManager.Users);
        }
        public ActionResult DetailsUser(string id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        public async Task<ActionResult> DeleteUser(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost, ActionName("DeleteUser")]
        public async Task<ActionResult> DeleteUserConfirmed(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            await db.SaveChangesAsync();
            IdentityResult result = await UserManager.DeleteAsync(user);
            return RedirectToAction("GetUser");

        }
        public ActionResult CreateQuiz()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateQuiz(Quiz quiz, List<string> QuestionText, List<string> QuestionDescription)
        {
            db.Quizzes.Add(quiz);
            for(int i =0; i<QuestionText.Count;i++)
            {
                Question question = new Question
                {
                    QuestionText = QuestionText[i],
                    QuestionDescription = QuestionDescription[i],
                    QuizId = quiz.QuizId
                };
                db.Questions.Add(question);
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public ActionResult CreateQuestion()
        {
            return View();
        }
    }    
}