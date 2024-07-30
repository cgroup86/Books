using System.Runtime.CompilerServices;

namespace Books.BL
{
    public class Book
    {
        int id;
        string title; //The name of the book
        string subtitle; //A secondary title, if available
        List<Author> authors; //The names of the authors
        string publisher;
        DateTime publishedDate;
        int pageCount; //The number of pages in the book.
        List<Category> categories;
        // Check if needed
        string maturityRating;
        string coverImageUrl;
        string language;
        int price; //Price of the book;



        string previewLink; //Link to a preview or sample of the book
        string description;
        bool textToSpeechPermission;
        

        //Only for digital
        string webReaderLink; 
    }
}
