namespace Books.BL
{
    public class Author
    {
        string name;
        string topWork;
        int workCount;
        string key;
        string image;
        string? description;
        public Author(string name, string topWork, int workCount, string key, string images, string description)
        {
            this.Name = name;
            this.TopWork = topWork;
            this.WorkCount = workCount;
            this.Key = key;
            this.image = images;
            this.description = description;
        }
        public Author() { }

        public string Name { get => name; set => name = value; }
        public string TopWork { get => topWork; set => topWork = value; }
        public int WorkCount { get => workCount; set => workCount = value; }
        public string Key { get => key; set => key = value; }
        public string Image { get => image; set => image = value; }
        public string? Description { get => description; set => description = value; }

        public int Insert()
        {
            DBservices dBservices = new DBservices();
            return dBservices.insertAuthor(this);
        }

        public int InsertImagesOfAuthor(string name, string image, string description)
        {
            DBservices dBservices = new DBservices();
            return dBservices.insertImagesOfAuthors(name, image, description);
        }
        public List<Author> Read()
        {
            DBservices dBservices = new DBservices();
            return dBservices.readAuthors();
        }

        public List<Author> ReadAuthorsByPage(int pageNumber, int pageSize)
        {
            DBservices dBservices = new DBservices();
            return dBservices.readAuthorsByPage(pageNumber, pageSize);
        }

    }
}