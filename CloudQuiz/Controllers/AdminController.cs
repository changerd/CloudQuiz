using CloudQuiz.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
        public async Task<ActionResult> CreateQuiz(Quiz quiz, List<string> QuestionText, List<string> QuestionDescription, List<string> QuestionAnswer, List<HttpPostedFileBase> uploadQuestionImage)
        {
            db.Quizzes.Add(quiz);
            if (QuestionText != null)
            {
                for (int i = 0; i < QuestionText.Count; i++)
                {
                    Question question = new Question
                    {
                        QuestionText = QuestionText[i],
                        QuestionDescription = QuestionDescription[i],
                        QuestionAnswer = QuestionAnswer[i].ToLower(),
                        QuizId = quiz.QuizId
                    };
                    if (uploadQuestionImage[i] != null)
                    {
                        byte[] imageData = null;
                        using (var binaryReader = new BinaryReader(uploadQuestionImage[i].InputStream))
                        {
                            imageData = binaryReader.ReadBytes(uploadQuestionImage[i].ContentLength);
                        }
                        question.QuestionImage = imageData;
                    }
                    db.Questions.Add(question);
                }
            }
            if (ModelState.IsValid)
            {
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return CreateQuiz();
        }
        public ActionResult GetQuiz()
        {
            return View(db.Quizzes.ToList());
        }
        public ActionResult EditQuiz(int id)
        {
            Quiz quiz = db.Quizzes.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }
        [HttpPost]
        public async Task<ActionResult> EditQuiz(Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quiz).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("GetQuiz");
            }
            return EditQuiz(quiz.QuizId);
        }
        public ActionResult DeleteQuiz(int id)
        {
            Quiz quiz = db.Quizzes.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }
        [HttpPost, ActionName("DeleteQuiz")]
        public async Task<ActionResult> DeleteQuizConfirmed(int id)
        {
            Quiz quiz = db.Quizzes.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            db.Quizzes.Remove(quiz);
            await db.SaveChangesAsync();
            return RedirectToAction("GetQuiz");
        }
        public ActionResult DetailsQuiz(int id)
        {
            Quiz quiz = db.Quizzes.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }
        public ActionResult CreateQuestion(int qid)
        {
            Question question = new Question
            {
                QuizId = qid
            };            
            return View(question);
        }
        [HttpPost]
        public async Task<ActionResult> CreateQuestion(Question question, HttpPostedFileBase uploadQuestionImage)
        {
            question.QuestionAnswer = question.QuestionAnswer.ToLower();
            if (uploadQuestionImage != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadQuestionImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadQuestionImage.ContentLength);
                }
                question.QuestionImage = imageData;
            }
            if (ModelState.IsValid)
            {
                db.Questions.Add(question);
                await db.SaveChangesAsync();
                return RedirectToAction("DetailsQuiz", new { id = question.QuizId });
            }
            return (CreateQuestion(question.QuizId));
        }
        public ActionResult DetailsQuestion(int id)
        {
            var questions = db.Questions.Include(q => q.Choices).Include(q => q.Quiz).ToList();
            var question = questions.Find(i => i.QuestionId == id);
            if (question == null)
                return HttpNotFound();
            return View(question);
        }
        public ActionResult DeleteQuestion(int id)
        {
            var question = db.Questions.Find(id);
            if (question == null)
                return HttpNotFound();
            return View(question);
        }
        [HttpPost, ActionName("DeleteQuestion")]
        public async Task<ActionResult> DeleteQuestionConfimed(int id)
        {
            var question = db.Questions.Find(id);
            if (question == null)
                return HttpNotFound();
            db.Questions.Remove(question);
            await db.SaveChangesAsync();
            return RedirectToAction("DetailsQuiz", new { id = question.QuizId });
        }
        public ActionResult EditQuestion(int id)
        {
            var question = db.Questions.Find(id);
            if (question == null)
                return HttpNotFound();
            return View(question);
        }
        [HttpPost]
        public async Task<ActionResult> EditQuestion(Question question, HttpPostedFileBase uploadQuestionImage, bool? deleteQuestionImage)
        {
            question.QuestionAnswer = question.QuestionAnswer.ToLower();
            if (uploadQuestionImage != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadQuestionImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadQuestionImage.ContentLength);
                }
                question.QuestionImage = imageData;
            }
            if (deleteQuestionImage == true)
            {
                Array.Clear(question.QuestionImage, 0, question.QuestionImage.Length);
            }
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("DetailsQuestion", new { id = question.QuestionId });
            }
            return EditQuestion(question.QuestionId);
        }
        public ActionResult CreateChoice(int qid)
        {
            Choice choice = new Choice
            {
                QuestionId = qid,
            };
            return View(choice);
        }
        [HttpPost]
        public async Task<ActionResult> CreateChoice(Choice choice)
        {
            if (ModelState.IsValid)
            {
                db.Choices.Add(choice);
                await db.SaveChangesAsync();
                return RedirectToAction("DetailsQuestion", new { id = choice.QuestionId });
            }
            return CreateChoice(choice.ChoiceId);
        }
        public ActionResult EditChoice(int id)
        {
            var choice = db.Choices.Find(id);
            if (choice == null)
                return HttpNotFound();
            return View(choice);
        }
        [HttpPost]
        public async Task<ActionResult> EditChoice(Choice choice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(choice).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("DetailsQuestion", new { id = choice.QuestionId });
            }
            return EditChoice(choice.ChoiceId);
        }
        public ActionResult DeleteChoice(int id)
        {
            var choice = db.Choices.Find(id);
            if (choice == null)
                return HttpNotFound();
            return View(choice);
        }
        [HttpPost, ActionName("DeleteChoice")]
        public async Task<ActionResult> DeleteChoiceConfirmed(int id)
        {
            var choice = db.Choices.Find(id);
            if (choice == null)
                return HttpNotFound();
            db.Choices.Remove(choice);
            await db.SaveChangesAsync();
            return RedirectToAction("DetailsQuestion", new { id = choice.QuestionId });
        }
        public ActionResult PublishQuiz(int id)
        {
            List<ApplicationUser> users = db.Users.ToList();
            ViewBag.Users = users;
            var quiz = db.Quizzes.Find(id);
            return View(quiz);
        }
        [HttpPost]
        public async Task<ActionResult> PublishQuiz(Quiz quiz, string[] selectedUsers)
        {
            var quizz = db.Quizzes.Find(quiz.QuizId);
            quizz.Users.Clear();
            if (selectedUsers != null)
            {
                foreach(var item in db.Users.Where(u => selectedUsers.Contains(u.Id)))
                {
                    quizz.Users.Add(item);
                }
            }
            db.Entry(quizz).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("DetailsQuiz", new { id = quiz.QuizId });
        }
    }    
}