namespace Books.BL
{
    public class Question
    {
        string questionText;
        List<object> answers;
        string correctAnswer;
        string? correctAnswerImage;
        public string QuestionText { get; set; }
        public List<object> Answers { get; set; }
        public string CorrectAnswer { get; set; }
        public string? CorrectAnswerImage { get; set; }

        public static Question getQuestion1()
        {
            try
            {
                DBservices dBservices = new DBservices();
                return dBservices.GetQuestion1();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static Question getQuestion2()
        {
            try
            {
                DBservices dBservices = new DBservices();
                return dBservices.GetQuestion2();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Question getQuestion3()
        {
            try
            {
                DBservices dBservices = new DBservices();
                return dBservices.GetQuestion3();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Question getQuestion4()
        {
            try
            {
                DBservices dBservices = new DBservices();
                return dBservices.GetQuestion4();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Question getQuestion5()
        {
            try
            {
                DBservices dBservices = new DBservices();
                return dBservices.GetQuestion5();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}