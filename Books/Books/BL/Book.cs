using System.Runtime.CompilerServices;

namespace Books.BL
{
    public class Book
    {
        int id; // Identifier for the book
        string title; // The name of the book
        int price; // Price of the book
        List<Author> authors; // The names of the authors
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

        public Book(int id, string title, int price, List<Author> authors, string publisher, DateTime publishedDate, string description, bool textReading, int pageCount, List<Category> categories, int averageRating, int ratingsCount, string smallThumbnailUrl, string thumbnailUrl, string language, string previewLink, string infoLink, string canonicalVolumeLink, bool isEbook, bool embeddable, bool epubIsAvailable, string epubDownloadLink, bool pdfIsAvailable, string pdfDownloadLink, string webReaderLink, string googleBooksId, string etag, string selfLink)
        {
            this.Id = id;
            this.Title = title;
            this.Price = price;
            this.Authors = authors;
            this.Publisher = publisher;
            this.PublishedDate = publishedDate;
            this.Description = description;
            this.TextReading = textReading;
            this.PageCount = pageCount;
            this.Categories = categories;
            this.AverageRating = averageRating;
            this.RatingsCount = ratingsCount;
            this.SmallThumbnailUrl = smallThumbnailUrl;
            this.ThumbnailUrl = thumbnailUrl;
            this.Language = language;
            this.PreviewLink = previewLink;
            this.InfoLink = infoLink;
            this.CanonicalVolumeLink = canonicalVolumeLink;
            this.IsEbook = isEbook;
            this.Embeddable = embeddable;
            this.EpubIsAvailable = epubIsAvailable;
            this.EpubDownloadLink = epubDownloadLink;
            this.PdfIsAvailable = pdfIsAvailable;
            this.PdfDownloadLink = pdfDownloadLink;
            this.WebReaderLink = webReaderLink;
            this.GoogleBooksId = googleBooksId;
            this.Etag = etag;
            this.SelfLink = selfLink;
        }

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public int Price { get => price; set => price = value; }
        public List<Author> Authors { get => authors; set => authors = value; }
        public string Publisher { get => publisher; set => publisher = value; }
        public DateTime PublishedDate { get => publishedDate; set => publishedDate = value; }
        public string Description { get => description; set => description = value; }
        public bool TextReading { get => textReading; set => textReading = value; }
        public int PageCount { get => pageCount; set => pageCount = value; }
        public List<Category> Categories { get => categories; set => categories = value; }
        public int AverageRating { get => averageRating; set => averageRating = value; }
        public int RatingsCount { get => ratingsCount; set => ratingsCount = value; }
        public string SmallThumbnailUrl { get => smallThumbnailUrl; set => smallThumbnailUrl = value; }
        public string ThumbnailUrl { get => thumbnailUrl; set => thumbnailUrl = value; }
        public string Language { get => language; set => language = value; }
        public string PreviewLink { get => previewLink; set => previewLink = value; }
        public string InfoLink { get => infoLink; set => infoLink = value; }
        public string CanonicalVolumeLink { get => canonicalVolumeLink; set => canonicalVolumeLink = value; }
        public bool IsEbook { get => isEbook; set => isEbook = value; }
        public bool Embeddable { get => embeddable; set => embeddable = value; }
        public bool EpubIsAvailable { get => epubIsAvailable; set => epubIsAvailable = value; }
        public string EpubDownloadLink { get => epubDownloadLink; set => epubDownloadLink = value; }
        public bool PdfIsAvailable { get => pdfIsAvailable; set => pdfIsAvailable = value; }
        public string PdfDownloadLink { get => pdfDownloadLink; set => pdfDownloadLink = value; }
        public string WebReaderLink { get => webReaderLink; set => webReaderLink = value; }
        public string GoogleBooksId { get => googleBooksId; set => googleBooksId = value; }
        public string Etag { get => etag; set => etag = value; }
        public string SelfLink { get => selfLink; set => selfLink = value; }
    }
}