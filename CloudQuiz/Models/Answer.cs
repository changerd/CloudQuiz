using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CloudQuiz.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }
        [Required]
        [Display(Name = "Название")]
        public string AnswerText { get; set; }
        [Required]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}