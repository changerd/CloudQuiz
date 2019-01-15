using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudQuiz.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}