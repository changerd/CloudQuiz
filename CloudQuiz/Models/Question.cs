using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CloudQuiz.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        [Required]
        [Display(Name = "Содержание вопроса")]
        public string QuestionText { get; set; }
        [Display(Name = "Описание")]
        public string QuestionDescription { get; set; }
        [Display(Name = "Изображение")]
        public byte[] QuestionImage { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Choice> Choices { get; set; }
        public Question()
        {
            Answers = new List<Answer>();
            Choices = new List<Choice>();
        }
    }
}