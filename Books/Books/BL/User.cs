using System.Diagnostics.Contracts;

namespace Books.BL
{
    public class User
    {
        int id;
        string name;
        string email;
        string password;
        bool isAdmin;
        bool isActive;
        //private PersonalLibrary library;

        public User(int id, string name, string email, string password, bool isAdmin, bool isActive)
        {
            this.Id = id;
            this.Name = name;
            this.Email = email;
            this.Password = password;
            this.IsAdmin = isAdmin;
            this.IsActive = isActive;
            //this.Library = library;
        }

        public User () { }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public bool IsAdmin { get => isAdmin; set => isAdmin = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        //public PersonalLibrary Library { get => library; set => library = value; }

        public List<User> Read()
        {
            DBservices dBservices = new DBservices();
            return dBservices.ReadUsers();
        }

        public bool Register()
        {
            try
            {
                DBservices dbservices = new DBservices();
                dbservices.UserRegister(this);
                return true;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            
        }

        public bool Login()
        {
            DBservices dBservices = new DBservices();
            bool success = dBservices.UserLogin(this);
            if (!success)
            {
                throw new ArgumentException("Invalid email or password");
            }
            return true;
        }


    }
}
