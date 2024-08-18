namespace Books.BL
{
    public class Question
    {
        public string QuestionText { get; set; }
        public List<object> Answers { get; set; }
        public string CorrectAnswer { get; set; }
        public string? CorrectAnswerImage { get; set; }

        public static Question getQuestion1()
        {
            DBservices dBservices = new DBservices();
            return dBservices.GetQuestion1();
        }


        public static Question getQuestion2()
        {
            DBservices dBservices = new DBservices();
            return dBservices.GetQuestion2();
        }
    }
}