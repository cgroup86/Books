namespace Books.BL
{
    public class PersonalLibrary
    {
        int userId;
        int bookId;
        bool status; // "ToRead" = false or "HaveRead" = true
        bool isPurchased; // true = "purchased"

        

        public PersonalLibrary() { }

        public PersonalLibrary(int userId, int bookId, bool status, bool isPurchased)
        {
            this.UserId = userId;
            this.BookId = bookId;
            this.Status = status;
            this.IsPurchased = isPurchased;
        }

        public int UserId { get => userId; set => userId = value; }
        public int BookId { get => bookId; set => bookId = value; }
        public bool Status { get => status; set => status = value; }
        public bool IsPurchased { get => isPurchased; set => isPurchased = value; }

        public bool AddWantToRead(int bookId, int userId, bool status, bool isPurchased)
        {
            try
            {
                DBservices dBservices = new DBservices();
                //dBservices.AddWantToRead(bookId, userId, status, isPurchased);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
         
        }
    }
}
