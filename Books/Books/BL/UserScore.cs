namespace Books.BL
{
    public class UserScore
    {
        int userId;
        int score;
        List<object?> topUserScores;


        public UserScore(int userId, int score, List<object?> topUserScores)
        {
            this.UserId = userId;
            this.Score = score;
            this.TopUserScores = topUserScores;
        }

        public UserScore() { }

        public int UserId { get => userId; set => userId = value; }
        public int Score { get => score; set => score = value; }
        public List<object?> TopUserScores { get => topUserScores; set => topUserScores = value; }

        public int SubmitScore()
        {
            try
            {
                DBservices dBservices = new DBservices();
                return dBservices.SubmitScore(this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static UserScore GetUserAndTopScores(int id)
        {
            try
            {
                DBservices dbservices = new DBservices();
                return dbservices.GetUserAndTopScores(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
