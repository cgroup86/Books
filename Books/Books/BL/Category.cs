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

    }
}
