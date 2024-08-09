namespace Books.BL
{
    public class Category
    {
        string name;

        public Category(string name)
        {
            this.Name = name;
        }

        public Category() { }

        public string Name { get => name; set => name = value; }

        //public Category Insert()
        //{
        //    DBservices dBservices = new DBservices();
        //    return dBservices.insertAuthor(this);
        //}

        public List<User> insertCategories()
        {
            DBservices dBservices = new DBservices();
            return dBservices.readUsers();
        }
    }
}
