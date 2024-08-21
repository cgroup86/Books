namespace Books.BL
{
    public class Author
    {
        string name;
        string topWork;
        int workCount;
        string image;
        string? description;
        public Author(string name, string topWork, int workCount, string images, string description)
        {
            this.Name = name;
            this.TopWork = topWork;
            this.WorkCount = workCount;
            this.image = images;
            this.description = description;
        }
        public Author() { }

        public string Name { get => name; set => name = value; }
        public string TopWork { get => topWork; set => topWork = value; }
        public int WorkCount { get => workCount; set => workCount = value; }
        public string Image { get => image; set => image = value; }
        public string? Description { get => description; set => description = value; }

        public int Insert()
        {
            try
            {
                DBservices dBservices = new DBservices();
                return dBservices.insertAuthor(this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertImagesOfAuthor(string name, string image, string description)
        {
            try
            {
                DBservices dBservices = new DBservices();
                return dBservices.insertImagesOfAuthors(name, image, description);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Author> Read()
        {

            try
            {
                DBservices dBservices = new DBservices();
                return dBservices.readAuthors();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Author> ReadAuthorsByPage(int pageNumber, int pageSize)
        {
            try
            {
                DBservices dBservices = new DBservices();
                return dBservices.readAuthorsByPage(pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Book> ReadBooksByAuthorName(string authorName)
        {
            try
            {
                DBservices dBservices = new DBservices();
                return dBservices.readBooksByAuthorName(authorName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int getNumOfAuthorsInLibraries(string authorName)
        {
            try
            {
                DBservices dbservices = new DBservices();
                return dbservices.getNumberOfAuthorsInPrivateLibrary(authorName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}