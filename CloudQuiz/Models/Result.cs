using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CloudQuiz.Models
{
    public class Result
    {
        public int ResultId { get; set; }
        [Display (Name = "Оценка")]
        public int ResultScore { get; set; }
        [Display(Name = "Дата")]
        public DateTime ResultDate { get; set; }
        public int QuizId { get; set; }
        public string UserId { get; set; }
        public Quiz Quiz { get; set; }
        public ApplicationUser User { get; set; }

    }
}