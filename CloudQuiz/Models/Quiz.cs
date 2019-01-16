using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CloudQuiz.Models
{
    public class Quiz
    {
        public int QuizId { get; set; }
        [Required]
        [Display(Name = "Название")]
        public string QuizName { get; set; }
        [Display(Name = "Длительность")]
        public TimeSpan? QuizDuration { get; set; }
        [Display(Name = "Описание")]
        public string QuizDescription { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Result> Results { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public Quiz()
        {
            Questions = new List<Question>();
            Results = new List<Result>();
            Users = new List<ApplicationUser>();
        }
    }
}