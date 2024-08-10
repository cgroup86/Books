namespace Books.BL
{
    public class Author
    {
        string name;
        string topWork;
        int workCount;
        string key;

        public Author(string name, string topWork, int workCount, string key)
        {
            this.Name = name;
            this.TopWork = topWork;
            this.WorkCount = workCount;
            this.Key = key;
        }
        public Author() { }

        public string Name { get => name; set => name = value; }
        public string TopWork { get => topWork; set => topWork = value; }
        public int WorkCount { get => workCount; set => workCount = value; }
        public string Key { get => key; set => key = value; }

        public int Insert()
        {
            DBservices dBservices = new DBservices();
            return dBservices.insertAuthor(this);
        }

        //public List<Author> Read()
        //{
        //    DBservices dBservices = new DBservices();
        //    return dBservices.readAuthors();
        //}
    }
}
