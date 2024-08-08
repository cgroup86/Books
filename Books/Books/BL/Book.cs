using System.Runtime.CompilerServices;

namespace Books.BL
{
    public class Book
    {
        int id; // Identifier for the book
        string title; // The name of the book
        int price; // Price of the book
        List<string> authors; // The names of the authors
        string publisher;
        DateTime publishedDate;
        string description;
        bool textReading; // If book available in text format
        int pageCount; // The number of pages in the book.
        List<Category> categories;
        // TODO - check if the rating needed
        int averageRating;
        int ratingsCount;

        string smallThumbnailUrl;
        string thumbnailUrl;
        string language;
        // TODO - check if the 3 links needed
        string previewLink; 
        string infoLink;
        string canonicalVolumeLink;

        bool isEbook; // True = digital, false = physical

        // For digital
        bool embeddable;
        bool epubIsAvailable;
        string epubDownloadLink;
        bool pdfIsAvailable;
        string pdfDownloadLink;
        string webReaderLink;


        // Id and things on google api books
        string googleBooksId;
        string etag;
        string selfLink;

    }
}