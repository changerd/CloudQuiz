using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudQuiz.Models
{
    public class Result
    {
        public int ResultId { get; set; }
        public int ResultScore { get; set; }
        public DateTime ResultDate { get; set; }
        public int QuizId { get; set; }
        public int UserId { get; set; }
        public Quiz Quiz { get; set; }
        public ApplicationUser User { get; set; }

    }
}