using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CloudQuiz.Models
{
    public class Choice
    {
        public int ChoiceId { get; set; }
        [Required]
        [Display(Name ="Название")]
        public string ChoiceText { get; set; }
        [Required]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}