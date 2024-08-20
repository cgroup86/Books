using Microsoft.AspNetCore.Server.IIS.Core;
using System.Data;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Books.BL
{
    public class Book
    {
        int id; // Identifier for the book
        string title; // The name of the book
        int price; // Price of the book
        List<string>? authors; // The names of the authors
        string? description;
        int pageCount; // The number of pages in the book.
        List<string>? categories;
        string smallThumbnailUrl;
        string language;
        string previewLink;
        bool isEbook; // True = digital, false = physical
        bool embeddable;
        string webReaderLink;
        string googleBooksId;

        bool? isActive;
        bool isAvailable; // For physical book, 1 available for sale, 0 not available for sale
        int numOfPrints; // related physical only
        public Book() { }

        public Book(int id, string title, int price, List<string>? authors, string? description, int pageCount, List<string>? categories, string smallThumbnailUrl, string language, string previewLink, bool isEbook, bool embeddable, string webReaderLink, string googleBooksId, bool? isActive, bool isAvailable, int numOfPrints)
        {
            this.id = id;
            this.title = title;
            this.price = price;
            this.authors = authors;
            this.description = description;
            this.pageCount = pageCount;
            this.categories = categories;
            this.smallThumbnailUrl = smallThumbnailUrl;
            this.language = language;
            this.previewLink = previewLink;
            this.isEbook = isEbook;
            this.embeddable = embeddable;
            this.webReaderLink = webReaderLink;
            this.googleBooksId = googleBooksId;
            this.isActive = isActive;
            this.isAvailable = isAvailable;
            this.numOfPrints = numOfPrints;
        }

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public int Price { get => price; set => price = value; }
        public List<string>? Authors { get => authors; set => authors = value; }
        public string? Description { get => description; set => description = value; }
        public int PageCount { get => pageCount; set => pageCount = value; }
        public List<string>? Categories { get => categories; set => categories = value; }
        public string SmallThumbnailUrl { get => smallThumbnailUrl; set => smallThumbnailUrl = value; }
        public string Language { get => language; set => language = value; }
        public string PreviewLink { get => previewLink; set => previewLink = value; }
        public bool IsEbook { get => isEbook; set => isEbook = value; }
        public bool Embeddable { get => embeddable; set => embeddable = value; }
        public string WebReaderLink { get => webReaderLink; set => webReaderLink = value; }
        public string GoogleBooksId { get => googleBooksId; set => googleBooksId = value; }
        public bool? IsActive { get => isActive; set => isActive = value; }
        public bool IsAvailable { get => isAvailable ; set => isAvailable = value; }
        public int NumOfPrints { get => numOfPrints; set => numOfPrints = value; }

        public void EnsureDefaults()
        {
            Title = Title ?? string.Empty;
            Description = Description ?? string.Empty;
            SmallThumbnailUrl = SmallThumbnailUrl ?? string.Empty;
            Language = Language ?? string.Empty;
            PreviewLink = PreviewLink ?? string.Empty;
            WebReaderLink = WebReaderLink ?? string.Empty;
            GoogleBooksId = GoogleBooksId ?? string.Empty;
            Price = Price == null ? 0 : Price;
            PageCount = PageCount == null ? 0 : PageCount;
            IsEbook = IsEbook == null ? false : IsEbook;
            Embeddable = Embeddable == null ? false : Embeddable;
            IsActive = IsActive == null ? true : IsActive;

            Categories = Categories ?? new List<string>();
            Authors = Authors ?? new List<string>();
        }

        public int InsertBook()
        {
            try
            {
                EnsureDefaults();
                DBservices dBservices = new DBservices();
                int bookId = dBservices.InsertBook(this);

                for (int i = 0; i < this.Categories.Count(); i++)
                {
                    dBservices.InsertBookCategory(this.Categories[i], bookId);
                }

                for (int i = 0; i < this.Authors.Count(); i++)
                {
                    dBservices.InsertBookAuthor(this.Authors[i], bookId);
                }
                return bookId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Book> Read(bool isEbook, int pageNumber, int pageSize, out int totalRecords, bool fetchTotalCount)
        {
            try
            {
                DBservices dBservices = new DBservices();
                List<Book> books = dBservices.GetPagedBooks(isEbook, pageNumber, pageSize, out totalRecords, fetchTotalCount);
                // Loop through each book and assign authors
                //foreach (Book book in books)
                //{
                //    book.Authors = dBservices.GetAuthorsForBook(book.Id);
                //}
                return books;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Book> ReadAllBooks(int pageNumber, int pageSize)
        {
            try
            {
                DBservices dbservices = new DBservices();
                return dbservices.readBooksByPage(pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateBooksValues(int bookId, bool isActive, int newPrice, int numberOfPrints)
        {
            try
            {
                DBservices dbservices = new DBservices();
                return dbservices.UPdateBookValues(bookId, isActive, newPrice, numberOfPrints);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Book> GetRandom5Books()
        {
            try
            {
                DBservices dbservices = new DBservices();
                return dbservices.GetRandom5Books();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static List<Book> GetSearchedBooks(int searchType, string searchValue)
        {
            try
            {
                DBservices dBservices = new DBservices();
                return dBservices.GetSearchedBooks(searchType, searchValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int getNumOfBooksInLibraries(int bookId)
        {
            try
            {
                DBservices dbservices = new DBservices();
                return dbservices.getNumberOfBooksInPrivateLibrary(bookId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}