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

        public bool AddToLibrary()
        {
            try
            {
                DBservices dBservices = new DBservices();
                return dBservices.AddToLibrary(this);
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static void UpdateBookStatus(int userId, int bookId, bool newStatus, bool isEbook)
        {

            try
            {
                DBservices dbs = new DBservices();
                dbs.UpdateBookStatus(userId, bookId, newStatus, isEbook);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Error updating book status: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred: " + ex.Message, ex);
            }
        }

        public static List<object> GetBooksToRead(int userId)
        {
            try
            {
                DBservices dbs = new DBservices();
                return dbs.GetBooksToRead(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred: " + ex.Message, ex);
            }
        }

        public static List<object> GetBooksRead(int userId)
        {
            try
            {
                DBservices dbs = new DBservices();
                return dbs.GetBooksRead(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred: " + ex.Message, ex);
            }
        }

        public static List<object> GetBooksPurchased(int userId)
        {
            try
            {
                DBservices dbs = new DBservices();
                return dbs.GetBooksPurchased(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred: " + ex.Message, ex);
            }
        }

        public static int AddRemoveRequestToBuy(int sellerId, int buyerId, int bookId)
        {
            try
            {
                DBservices dbs = new DBservices();
                return dbs.AddRemoveRequestToBuy(sellerId, buyerId, bookId);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred: " + ex.Message, ex);
            }

        }

        public static int AcceptRequestToBuy(int sellerId, int buyerId, int bookId)
        {
            try
            {
                DBservices dbs = new DBservices();
                return dbs.AcceptRequestToBuy(sellerId, buyerId, bookId);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred: " + ex.Message, ex);
            }
        }

        public static List<object> GetPurchasedBooksWithStatus1(int userId)
        {
            try
            {
                DBservices dbs = new DBservices();
                return dbs.GetPurchasedBooksWithStatus1(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred: " + ex.Message, ex);
            }
        }

        public static List<object> GetRequestedBooksByBuyer(int userId)
        {
            try
            {
                DBservices dbs = new DBservices();
                return dbs.GetRequestedBooksByBuyer(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred: " + ex.Message, ex);
            }
        }

        public static void RejectRequestToBuyProcedure(int sellerId, int buyerId, int bookId)
        {
            try
            {
                DBservices dbs = new DBservices();
                dbs.RejectRequestToBuyProcedure(sellerId, buyerId, bookId);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred: " + ex.Message, ex);
            }
        }

        public static List<object> GetMyRequests(int buyerId)
        {
            try
            {
                DBservices dbs = new DBservices();
                return dbs.GetMyRequests(buyerId);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred: " + ex.Message, ex);
            }
        }

        public static List<object> GetWhoOwnTheBook(int bookId)
        {
            try
            {
                DBservices dbs = new DBservices();
                return dbs.GetWhoOwnTheBook(bookId);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred: " + ex.Message, ex);
            }
        }
    }
}
